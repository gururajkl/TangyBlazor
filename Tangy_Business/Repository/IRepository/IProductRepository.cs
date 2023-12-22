using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface IProductRepository
    {
        ProductDTO Create(ProductDTO productDTO);
        ProductDTO Update(ProductDTO productDTO);
        int Delete(int id);
        ProductDTO Get(int id);
        IEnumerable<ProductDTO> GetAll();
    }
}
