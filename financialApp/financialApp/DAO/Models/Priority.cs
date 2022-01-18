using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace financialApp.DAO.Models;


[JsonConverter(typeof(StringEnumConverter))]
public enum Priority
{
    Low,
    Medium,
    High,
}
