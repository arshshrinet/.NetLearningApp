using LearningApp.Model;
using LearningApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Services
{
    public class ProductService(UserDbContext context) : IProductService
    {
        public async Task<Product?> CreateProductAsync(Product product)
        {
            if (await context.Products.AnyAsync(u => u.Id == product.Id))
            {
                return null;
            }
            context.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            if(await context.Products.AnyAsync(u => u.Id == id))
            {
                var product = await context.Products.FindAsync(id);
                if (product != null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Product?>> GetProductAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var result = await context.Products.FindAsync(id);
            if(result is null) 
            { 
                return null;
            }
            return result;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            if(await context.Products.AnyAsync(u => u.Id == product.Id))
            {
                context.Update(product);
                await context.SaveChangesAsync();
                return product;
            }
            return null;
        }
    }
}
