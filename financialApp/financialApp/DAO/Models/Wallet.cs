namespace financialApp.DAO.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Name";
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}