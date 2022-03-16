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

        public static async Task AddToListInJsonFile(this HttpRequest request, string filePath, List<Product> products)
        {
            // find list
            // open list
            // convert from json to List
            // add to list
            // convert from List to json
            // save/overwrite file
        }

    }
}
