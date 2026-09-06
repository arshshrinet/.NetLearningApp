namespace LearningApp.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Decimal Price { get; set; }
        public Decimal Rating { get; set; }
        public int StockQuantity {  get; set; }

    }
}
