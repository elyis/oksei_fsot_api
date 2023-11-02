using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace oksei_fsot_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        Director,
        Appraiser,
        Teacher,
        Admin,
        Organization
    }
}