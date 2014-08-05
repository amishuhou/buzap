using System;
using System.Globalization;
using Newtonsoft.Json;

namespace UsedParts.Domain.Converters
{
    public class DateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //return (objectType == typeof(DateTime));
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var s = serializer.Deserialize<string>(reader);
            if (string.IsNullOrEmpty(s))
                return DateTime.MinValue;
            DateTime date;
            DateTime.TryParseExact(s, "dd.MM.yyyy", null, DateTimeStyles.None, out date);
            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Left as an exercise to the reader :)
            throw new NotImplementedException();
        }
    }
}
