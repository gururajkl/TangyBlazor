using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        CategoryDTO Create(CategoryDTO categoryDTO);
        CategoryDTO Update(CategoryDTO categoryDTO);
        int Delete(int id);
        CategoryDTO Get(int id);
        IEnumerable<CategoryDTO> GetAll();
    }
}
