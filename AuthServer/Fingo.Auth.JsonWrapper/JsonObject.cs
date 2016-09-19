using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fingo.Auth.JsonWrapper
{
    public class JsonObject
    {
        public string Result { get; set; }
        public string Jwt { get; set; }
        public string ErrorDetails { get; set; }

        public string ToJson()
        {
            JTokenWriter writer = new JTokenWriter();

            writer.WriteStartObject();

            writer.WritePropertyName("result");
            writer.WriteValue(Result);

            if (Result == JsonValues.Authenticated)
            {
                writer.WritePropertyName("jwt");
                writer.WriteValue(Jwt);
            }
            else if (Result == JsonValues.Error)
            {
                writer.WritePropertyName("errordetails");
                writer.WriteValue(ErrorDetails);
            }
            
            writer.WriteEndObject();
            return writer.Token.ToString(Formatting.None);
        }
    }
}