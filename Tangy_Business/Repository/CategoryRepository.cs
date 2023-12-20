using AutoMapper;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    /// <summary>
    /// CRUD operation of the category model.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper autoMapper;

        public CategoryRepository(ApplicationDbContext dbContext, IMapper autoMapper)
        {
            this.dbContext = dbContext;
            this.autoMapper = autoMapper;
        }

        /// <summary>
        /// Creates the category in the database.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO Model.</param>
        /// <returns></returns>
        public CategoryDTO Create(CategoryDTO categoryDTO)
        {
            // Manual conversion.
            Category categoryForDatabase = new()
            {
                Name = categoryDTO.Name,
                Id = categoryDTO.Id,
                CreatedDate = DateTime.Now
            };

            dbContext.Categories!.Add(categoryForDatabase);
            dbContext.SaveChanges();

            return new CategoryDTO
            {
                Name = categoryForDatabase.Name,
                Id = categoryForDatabase.Id
            };
        }

        /// <summary>
        /// Deletes the Category data from the database.
        /// </summary>
        /// <param name="id">ID of the category</param>
        /// <returns></returns>
        public int Delete(int id)
        {
            Category category = dbContext.Categories!.FirstOrDefault(c => c.Id == id)!;

            if (category != null)
            {
                dbContext.Categories!.Remove(category);
                return dbContext.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Returns the one category data.
        /// </summary>
        /// <param name="id">ID of the category</param>
        /// <returns></returns>
        public CategoryDTO Get(int id)
        {
            Category categoryFromDatabase = dbContext.Categories!.FirstOrDefault(c => c.Id == id)!;

            if (categoryFromDatabase != null)
            {
                // Using AutoMapper conversion.
                CategoryDTO categoryDTO = autoMapper.Map<Category, CategoryDTO>(categoryFromDatabase);
                return categoryDTO;
            }
            return new();
        }

        /// <summary>
        /// Returns all entire category.
        /// </summary>
        /// <returns>IEnumerable<CategoryDTO></returns>
        public IEnumerable<CategoryDTO> GetAll()
        {
            return autoMapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(dbContext.Categories!);
        }

        /// <summary>
        /// Updates the existing category object in database.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object</param>
        /// <returns></returns>
        public CategoryDTO Update(CategoryDTO categoryDTO)
        {
            Category categoryFromDB = dbContext.Categories!.FirstOrDefault(c => c.Id == categoryDTO.Id)!;

            if (categoryFromDB != null)
            {
                categoryFromDB.Name = categoryDTO.Name;
                dbContext.Categories!.Update(categoryFromDB);
                dbContext.SaveChanges();
                return autoMapper.Map<Category, CategoryDTO>(categoryFromDB);
            }

            return categoryDTO;
        }
    }
}
