namespace financialApp.DAO.Models;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
}
