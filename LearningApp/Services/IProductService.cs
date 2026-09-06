using LearningApp.Model;

namespace LearningApp.Services
{
    public interface IProductService
    {
        Task<Product?> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task<List<Product?>> GetProductAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<bool> DeleteProductByIdAsync(Guid id);
    }
}