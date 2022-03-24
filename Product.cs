using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTask

{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public override string ToString() => JsonSerializer.Serialize(this);
    }
}