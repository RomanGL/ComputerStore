using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ComputerStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Intel Core i5-6500",
                        Description = "A high performance Intel desktop processor.",
                        Category = "CPU",
                        Price = 11990
                    },
                    new Product
                    {
                        Name = "AMD Ryzen 7 1800X",
                        Description = "A high performance AMD desktop processor.",
                        Category = "CPU",
                        Price = 8000
                    },
                    new Product
                    {
                        Name = "Asus B150 Pro Gaming Aura",
                        Description = "An Asus LGA1151 motherboard with Aura lighting.",
                        Category = "Motherboard",
                        Price = 9100
                    },
                    new Product
                    {
                        Name = "Corsair K70 Red",
                        Description = "An USB mechanical Cherry MX Red keyboard with red lighting.",
                        Category = "Keyboard",
                        Price = 7890
                    }, 
                    new Product
                    {
                        Name = "Prestigio Smartbook 133s",
                        Description = "A light and powerfull laptop for home and office use.",
                        Category = "Laptop",
                        Price = 14000
                    },
                    new Product
                    {
                        Name = "Dell Inspiron 15 7566",
                        Description = "A gaming laptop for latest AAA PC Games.",
                        Category = "Laptop",
                        Price = 51990
                    },
                    new Product
                    {
                        Name = "Apacer 32 GB USB3.1",
                        Description = "A fast USB 3.1 flash drive with 32 GB capacity.",
                        Category = "USB flash drive",
                        Price = 790
                    },
                    new Product
                    {
                        Name = "WD 3200BEKT 320 GB",
                        Description = "A Western Digital 2.5\" HDD with 320 GB capacity.",
                        Category = "HDD",
                        Price = 2450
                    },
                    new Product
                    {
                        Name = "Apple AirPods",
                        Description = "A stylish wireless white headphones.",
                        Category = "Headphones",
                        Price = 14990
                    },
                    new Product
                    {
                        Name = "RedSquare Elite",
                        Description = "A gaming computer mouse with RGB lighting.",
                        Category = "Mouse",
                        Price = 1450
                    });

                context.SaveChanges();
            }
        }
    }
}
