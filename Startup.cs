using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using ApiTask.json;

namespace ApiTask
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string folder = "C:/Users/t.ibragimov/source/repos/ApiTask/json/";
            string jsonList = "listOne";

            app.Run(async context =>
            {
                DirectoryInfo dr = new DirectoryInfo("/json");
                var path = context.Request.Path;
                var request = context.Request;
                var response = context.Response;
                if (path == "/create/product" && request.Method == "POST")
                {
                    await request.CreateObjectToJsonFile(folder);
                }
                if (path == "/create/productlist" && request.Method == "POST")
                {
                    await request.CreateObjectListToJsonFile(folder);
                }
                if (path == "/addtoproductlist" && request.Method == "POST")
                {
                    if (!request.Headers.ContainsKey("listname"))
                    {
                        string[] fileArray = Directory.GetFiles("C:/Users/t.ibragimov/source/repos/ApiTask/json/");
                        foreach (string file in fileArray)
                        {
                            await response.WriteAsync($"{file}");
                        }
                    }
                    else
                    {
                        //i know it sux big time...but time was of the essence
                        var fileName = request.Headers.Single(h => h.Key == "listname").ToString().Replace("[","").Replace("]","").Replace("listname","").Replace(", ","");
                        await request.AddToListInJsonFile(fileName);
                    }
                }
                if (path == "/product" && request.Method == "GET")
                {
                    await request.GetObjectFromJsonFileById(response, folder);
                    
                }
                if (path == "/product" && request.Method == "PUT")
                {
                    await request.EditObjectInJsonFileById(response, folder);
                }
                if (path == "/product" && request.Method == "DELETE")
                {
                    await request.DeleteObjectInJsonFileById(response, folder);
                }
            });
        }
    }
}
