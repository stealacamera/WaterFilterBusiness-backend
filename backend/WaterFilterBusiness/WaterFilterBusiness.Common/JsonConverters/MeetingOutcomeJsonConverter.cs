﻿using System.Text.Json;
using System.Text.Json.Serialization;
using WaterFilterBusiness.Common.Enums;

namespace WaterFilterBusiness.Common.JsonConverters;

public sealed class MeetingOutcomeJsonConverter : JsonConverter<MeetingOutcome>
{
    public override MeetingOutcome? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        MeetingOutcome outcome;

        if (!MeetingOutcome.TryFromName(reader.GetString(), ignoreCase: true, out outcome))
            throw new JsonException();

        return outcome;
    }

    public override void Write(Utf8JsonWriter writer, MeetingOutcome value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
