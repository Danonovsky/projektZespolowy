namespace financialApp.DAO.Models
{
    public class Entry
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public string Description { get; set; } = "";
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public Guid WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public EntryType EntryType { get; set; }
        public Priority Priority { get; set; }
    }
}
