using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConsultaPopularApi.Converters
{
    public class DateTimeConverterUsingDateOnly : JsonConverter<DateTime>
    {
        private readonly string[] _formats = new[]
{
    "yyyy-MM-ddTHH:mm",           
    "yyyy-MM-ddTHH:mm:ss",        
    "yyyy-MM-ddTHH:mm:ss.fffZ",   
    "dd/MM/yyyy",
    "dd-MM-yyyy"
};

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            return DateTime.ParseExact(str, _formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_formats[0]));
        }
    }
}
