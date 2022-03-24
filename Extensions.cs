using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTask.json
{
    public static class Extensions
    {
        public static async Task CreateObjectToJsonFile(this HttpRequest request, string folder)
        {
            var newProduct = request.ReadFromJsonAsync<Product>();

            var product = new Product
            {
                Id = newProduct.Result.Id,
                Name = newProduct.Result.Name
            };

            var fileName = $"{folder}{newProduct.Result.Name}.json";

            using FileStream fileStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(fileStream, product);
        }

        public static async Task CreateObjectListToJsonFile(this HttpRequest request, string folder)
        {
            Random random = new Random();
            var jsonListName = $"List_{random.Next()}";
            var path = $"{folder}{jsonListName}";

            var newProducts = request.ReadFromJsonAsync<List<Product>>();
            var products = newProducts.Result; //is like List<Product> products

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fileStream, products);
        }

        public static async Task AddToListInJsonFile(this HttpRequest request, string fileName)
        {
            //it works, but need to put new data in the same [] as the data before

            var newProducts = request.ReadFromJsonAsync<List<Product>>();
            var products = newProducts.Result;

            using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
            {
                await JsonSerializer.SerializeAsync(fileStream, products);
            }
        }

        public static async Task GetObjectFromJsonFileById(this HttpRequest request, HttpResponse response, string folder)
        {
            var getRequestData = request.ReadFromJsonAsync<Product>();
            var name = getRequestData.Result.Name;
            var getDisplayData = getRequestData.Result;
            var fileName = $"{folder}{name}.json";

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                var product = JsonSerializer.DeserializeAsync<Product>(fileStream);
                //await response.SendFileAsync(fileName);
                await response.WriteAsJsonAsync<Product>(getDisplayData);
            }
        }
        public static async Task EditObjectInJsonFileById(this HttpRequest request, HttpResponse response, string folder)
        {
            var putRequestData = request.ReadFromJsonAsync<Product>();
            var name = putRequestData.Result.Name;
            var fileName = $"{folder}{name}.json";

            var product = new Product
            {
                Id = putRequestData.Result.Id,
                Name = putRequestData.Result.Name
            };

            File.Delete(fileName);

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<Product>(fileStream, product);

                await response.WriteAsync($"id changed to {product.Id}");
            }
        }
        public static async Task DeleteObjectInJsonFileById(this HttpRequest request, HttpResponse response, string folder)
        {
            var putRequestData = request.ReadFromJsonAsync<Product>();
            var name = putRequestData.Result.Name;
            var fileName = $"{folder}{name}.json";

            File.Delete(fileName);

            await response.WriteAsync($"{fileName} deleted");
        }


    }
}
