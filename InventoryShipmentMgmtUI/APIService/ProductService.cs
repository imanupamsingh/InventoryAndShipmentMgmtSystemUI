using InventoryManagementSystemUI.Common;
using InventoryShipmentMgmtUI.Model;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Security;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InventoryManagementSystemUI.ApiServices
{
    public class ProductService
    {
        private readonly HttpClient httpClients;
        private string apiUrl = "";

        public ProductService()
        {
            httpClients = new HttpClient();
            apiUrl = string.Empty;
            httpClients.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
            (sender, certificate, chain, sslPolicyErrors) => true);
        }

        // GET: Get all products
        public async Task<ProductApiResponse> GetProductsAsync()
        {
            apiUrl = CommonCode._baseUrl + CommonCode.GetAllProducts;            
            HttpResponseMessage response = await httpClients.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductApiResponse>(responseBody);
        }

        // POST: Create a new product
        public async Task<APIResponseModel> CreateProductAsync(ProductRequest productRequest)
        {
            apiUrl = CommonCode._baseUrl + CommonCode.AddNewProduct;
            string json = JsonConvert.SerializeObject(productRequest);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClients.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<APIResponseModel>(responseBody);
        }

        // PUT: Update a product
        public async Task<APIResponseModel> UpdateProductAsync(ProductRequest productRequest)
        {
            apiUrl = CommonCode._baseUrl + CommonCode.UpdateProduct;
            string json = JsonConvert.SerializeObject(productRequest);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClients.PutAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<APIResponseModel>(responseBody);
        }
        // DELETE: Delete a product
        public async Task<APIResponseModel> DeleteProductAsync(int productId)
        {
            apiUrl = CommonCode._baseUrl + CommonCode.DeleteProduct + $"?productId";
            HttpResponseMessage response = await httpClients.DeleteAsync($"{apiUrl}={productId}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<APIResponseModel>(responseBody);
        }

        // GET: Get product details by productId
        public async Task<ProductModel> GetProductById(int productId)
        {
            apiUrl = CommonCode._baseUrl + CommonCode.GetProductById+ $"?productId";
            HttpResponseMessage response = await httpClients.GetAsync($"{apiUrl}={productId}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductModel>(responseBody);
        }
    }
}
