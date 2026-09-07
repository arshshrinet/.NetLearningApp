using LearningApp.Model;
using LearningApp.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Reflection.Metadata;

namespace LearningApp.Services
{
    public class ProductService(UserDbContext context, IMemoryCache cache) : IProductService
    {
        private readonly string cacheKey = "ProductData";
        public async Task<Product?> CreateProductAsync(Product product)
        {
            if (await context.Products.AnyAsync(u => u.Id == product.Id))
            {
                return null;
            }
            context.Add(product);
            await context.SaveChangesAsync();
            if (cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts is not null)
            {
                cachedProducts.Add(product);
            }
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
                    if (cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts is not null)
                    {
                        cachedProducts.RemoveAll(p => p.Id == product.Id);
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Product>> GetProductAsync()
        {
            var products = await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(120));
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                entry.SetPriority(CacheItemPriority.High);
                entry.SetSize(2048);
                return await context.Products.AsNoTracking().ToListAsync();
            });
            return products ?? [];
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            if(cache.TryGetValue(id, out Product? product))
            {
                return product;
            }
            var result = await context.Products.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if(result is not null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(2048)
                    .RegisterPostEvictionCallback((key, value, reason, state) =>
                    {
                        Log.Information("Cache entry evicted. Key: {CacheKey}, Reason: {Reason}.", key, reason);
                    });
                cache.Set(id,result,cacheOptions);
            }
            return result;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            if(await context.Products.AnyAsync(u => u.Id == product.Id))
            {
                context.Update(product);
                await context.SaveChangesAsync();
                if (cache.TryGetValue(cacheKey, out List<Product>? cachedProducts) && cachedProducts is not null)
                {
                    cachedProducts.RemoveAll(p => p.Id == product.Id);
                    cachedProducts.Add(product);
                }
                return product;
            }
            return null;
        }
    }
}
