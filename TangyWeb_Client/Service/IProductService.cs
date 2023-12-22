using Tangy_Models;

namespace TangyWeb_Client.Service
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAllProducts();
        public Task<ProductDTO> GetProduct(int id);
    }
}
