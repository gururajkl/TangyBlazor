using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    /// <summary>
    /// CRUD operation of the product model.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper autoMapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper autoMapper)
        {
            this.dbContext = dbContext;
            this.autoMapper = autoMapper;
        }

        /// <summary>
        /// Creates the product in the database.
        /// </summary>
        /// <param name="productDTO">ProductDTO Model.</param>
        /// <returns>ProductDTO</returns>
        public ProductDTO Create(ProductDTO productDTO)
        {
            // Manual conversion.
            Product productForDatabase = autoMapper.Map<ProductDTO, Product>(productDTO);

            dbContext.Products!.Add(productForDatabase);
            dbContext.SaveChanges();

            return autoMapper.Map<Product, ProductDTO>(productForDatabase);
        }

        /// <summary>
        /// Deletes the Product data from the database.
        /// </summary>
        /// <param name="id">ID of the product</param>
        /// <returns>int - save changes</returns>
        public int Delete(int id)
        {
            Product product = dbContext.Products!.FirstOrDefault(c => c.Id == id)!;

            if (product != null)
            {
                dbContext.Products!.Remove(product);
                return dbContext.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// Returns the one product data.
        /// </summary>
        /// <param name="id">ID of the product</param>
        /// <returns>ProductDTO</returns>
        public ProductDTO Get(int id)
        {
            Product productFromDatabase = dbContext.Products!.Include(u => u.Category).FirstOrDefault(c => c.Id == id)!;

            if (productFromDatabase != null)
            {
                // Using AutoMapper conversion.
                ProductDTO productDTO = autoMapper.Map<Product, ProductDTO>(productFromDatabase);
                return productDTO;
            }
            return new();
        }

        /// <summary>
        /// Returns all entire product.
        /// </summary>
        /// <returns>IEnumerable<ProductDTO></returns>
        public IEnumerable<ProductDTO> GetAll()
        {
            return autoMapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(dbContext.Products!.Include(u => u.Category));
        }

        /// <summary>
        /// Updates the existing product object in database.
        /// </summary>
        /// <param name="productDTO">ProductDTO object</param>
        /// <returns></returns>
        public ProductDTO Update(ProductDTO productDTO)
        {
            Product productFromDB = dbContext.Products!.FirstOrDefault(c => c.Id == productDTO.Id)!;

            if (productFromDB != null)
            {
                productFromDB.Name = productDTO.Name;
                productFromDB.Description = productDTO.Description;
                productFromDB.ShopFavorites = productDTO.ShopFavorites;
                productFromDB.CustomerFavorites = productDTO.CustomerFavorites;
                productFromDB.Color = productDTO.Color;
                productFromDB.ImageUrl = productDTO.ImageUrl;
                productFromDB.CategoryId = productDTO.CategoryId;

                dbContext.Products!.Update(productFromDB);
                dbContext.SaveChanges();
                return autoMapper.Map<Product, ProductDTO>(productFromDB);
            }

            return productDTO;
        }
    }
}
