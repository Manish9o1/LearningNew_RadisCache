using RappidMQ.Models;
using RappidMQ.IRepo;
using RappidMQ.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
namespace RappidMQ.Repo
{
    public class Repo : IRepoInterFace
    {
        private EntityDbContext _DbContext;
        private readonly IDistributedCache _distributedCache;
        public Repo(EntityDbContext entityDbContext, IDistributedCache distributedCache)
        {
            _DbContext = entityDbContext;
            _distributedCache = distributedCache;
        }
        public async Task<string> PostProduct(Product product)
        {
            try
            {
                _DbContext.Products.Add(product);
                var productList = await _DbContext.Products.Select(x => x).ToListAsync();
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
               .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
               .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                // Serializing the data
                string cachedDataString = JsonSerializer.Serialize(productList);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                // Add the data into the cache
                await _distributedCache.SetAsync("product", dataToCache, options);
                await _DbContext.SaveChangesAsync();
                return "Product Is Created!";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Product>> GetAllProduct()
        {
            byte[]? cachedData = await _distributedCache.GetAsync("product");
            List<Product>? products = new();
            if (cachedData is not null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                products = JsonSerializer.Deserialize<List<Product>>(cachedDataString);
                return products.Select(x => x).ToList();
            }
            else
                return await _DbContext.Products.Select(x => x).ToListAsync();
        }
        public async Task<List<Product>?> GetProductFromCache(int productId)
        {
            byte[]? cachedData= await _distributedCache.GetAsync("product");
            List<Product>? products = new();
            if (cachedData is not null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                products = JsonSerializer.Deserialize<List<Product>>(cachedDataString);
                return products?.Where(x=>x.Id==productId).Select(x=>x).ToList();
            }
            else
            return await _DbContext.Products.Where(x => x.Id == productId).Select(x => x).ToListAsync();
        }

    }
}
