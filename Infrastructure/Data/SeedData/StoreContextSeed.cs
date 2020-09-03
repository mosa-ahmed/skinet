using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.SeedData
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            //Because we are gonna be running our seed method inside from our Program Class, we are not gonna have any global Exception Handler so wee need to run this inside Try{} Catch{}
            try
            {
                if (!context.ProductBrands.Any())
                {
                    //Because this is gonn abe running from our Program class which is inside our API project folder 
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    //Now we want to serialize what's inside json into our Product brands objects
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }
                    await context.SaveChangesAsync();
                }


                if (!context.ProductTypes.Any())
                {
                    //Because this is gonn abe running from our Program class which is inside our API project folder 
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    //Now we want to serialize what's inside json into our Product brands objects
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    //Because this is gonn abe running from our Program class which is inside our API project folder 
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                    //Now we want to serialize what's inside json into our Product brands objects
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}