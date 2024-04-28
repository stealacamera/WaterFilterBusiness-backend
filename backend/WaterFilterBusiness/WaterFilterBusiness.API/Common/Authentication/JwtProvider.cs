﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WaterFilterBusiness.BLL.Services.Identity;
using WaterFilterBusiness.Common.DTOs;
using WaterFilterBusiness.Common.Options;

namespace WaterFilterBusiness.API.Common.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateAsync(User user);
}

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly IPermissionsService _permissionsService;

    public JwtProvider(
        IOptions<JwtOptions> options,
        IPermissionsService permissionsService)
    {
        _options = options.Value;
        _permissionsService = permissionsService;
    }

    public async Task<string> GenerateAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var signingCredetials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredetials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
