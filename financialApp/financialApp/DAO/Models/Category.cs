namespace financialApp.DAO.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Guid? CategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual List<Category> Categories { get; set; } = new List<Category>();
        public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
