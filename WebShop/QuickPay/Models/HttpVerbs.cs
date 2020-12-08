using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebShop.QuickPay.Models
{
    // ReSharper disable InconsistentNaming
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HttpVerb
    {
        GET,
        POST,
        DELETE,
        PUT,
        PATCH
    }
}