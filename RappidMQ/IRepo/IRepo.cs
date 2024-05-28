using RappidMQ.Models;

namespace RappidMQ.IRepo
{
    public interface IRepoInterFace
    {
        public Task<string> PostProduct(Product product);
        public Task<List<Product>> GetAllProduct();
        public Task<List<Product>?> GetProductFromCache(int productId);
    }
}
