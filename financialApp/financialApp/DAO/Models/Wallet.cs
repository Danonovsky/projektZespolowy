namespace financialApp.DAO.Models;

public class Wallet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "Name";
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual List<Entry> Entries { get; set; } = new List<Entry>();
}

public class WalletOut
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<EntryOut> Entries { get; set; } = new List<EntryOut>();
    public Guid UserId { get; set; }
}
