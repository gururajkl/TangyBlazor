using Newtonsoft.Json;
using Tangy_Models;

namespace TangyWeb_Client.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly string baseServerUrl;

        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            baseServerUrl = configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var response = await httpClient.GetAsync("/api/product");
            var content = await response.Content.ReadAsStringAsync();

            // Check the status code of the response.
            if (response.IsSuccessStatusCode)
            {
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);

                foreach (var product in products!)
                {
                    product.ImageUrl = baseServerUrl + product.ImageUrl;
                }

                return products!;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel!.ErrorMessage);
            }
        }

        public async Task<ProductDTO> GetProduct(int id)
        {
            var response = await httpClient.GetAsync($"/api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductDTO>(content);
                product!.ImageUrl = baseServerUrl + product.ImageUrl;
                return product!;
            }
            else
            {
                return new ProductDTO();
            }
        }
    }
}
