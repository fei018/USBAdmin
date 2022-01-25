using Newtonsoft.Json;
using System;

namespace USBNotifyLib
{
    public class AbstractJsonConverter<TReal, TAbstract> : JsonConverter where TReal : TAbstract
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(TAbstract);

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer jser)
            => jser.Deserialize<TReal>(reader);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer jser)
            => jser.Serialize(writer, value);
    }
}
