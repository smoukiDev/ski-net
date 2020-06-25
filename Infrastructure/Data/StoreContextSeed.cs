using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Core.Entities;
using System;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        private readonly StoreContext _context;
        private readonly ILoggerFactory _loggerFactory;

        private const string SEED_DATA_DIR = "../Infrastructure/Data/SeedData/";
        public StoreContextSeed(StoreContext context, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _context = context;
        }

        private async Task SeedProducts()
        {
            if (!_context.Products.Any())
            {
                var path = $"{SEED_DATA_DIR}products.json";
                using(FileStream fs = File.OpenRead(path))
                {
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(fs);
                    foreach (var item in products)
                    {
                        _context.Products.Add(item);
                    }
                }
            }
        }
        private async Task SeedProductBrands()
        {
            if (!_context.ProductBrands.Any())
            {
                var path = $"{SEED_DATA_DIR}brands.json";
                using(FileStream fs = File.OpenRead(path))
                {
                    var brands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(fs);
                    foreach (var item in brands)
                    {
                        _context.ProductBrands.Add(item);
                    }
                }
            }
        }
        private async Task SeedProductTypes()
        {
            if (!_context.ProductTypes.Any())
            {
                var path = $"{SEED_DATA_DIR}types.json";
                using(FileStream fs = File.OpenRead(path))
                {
                    var types = await JsonSerializer.DeserializeAsync<List<ProductType>>(fs);
                    foreach (var item in types)
                    {
                        _context.ProductTypes.Add(item);
                    }
                }
            }
        }
        public async Task SeedAsync()
        {
            var logger = _loggerFactory.CreateLogger<StoreContextSeed>();
            try
            {
                await SeedProducts();
                await SeedProductBrands();
                await SeedProductTypes();
                await _context.SaveChangesAsync();
                logger.LogInformation("SeedData is success.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SeedData is failed.");
            }
        }
    }
}