using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace financialApp.DAO.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum EntryType
{
    Income,
    Expense,
    TransferIn,
    TransferOut,
}
