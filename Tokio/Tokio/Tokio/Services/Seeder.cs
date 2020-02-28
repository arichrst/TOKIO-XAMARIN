using System;
using System.Threading.Tasks;
using Tokio.Models;

namespace Tokio.Services
{
    public static class Seeder
    {
        public static async Task InitSeeder()
        {
            Database db = new Database();
            Category category1 = new Category();
            category1.Code = "CT01";
            category1.ImageUrl = "https://cdn.logojoy.com/wp-content/uploads/2018/05/01105857/1553.png";
            category1.Name = "FAST FOOD";
            await db.Categories.AddItemAsync(category1);

            Category category2 = new Category();
            category2.Code = "CT02";
            category2.ImageUrl = "https://cdn.logojoy.com/wp-content/uploads/2018/05/01105857/1553.png";
            category2.Name = "SLOW FOOD";
            await db.Categories.AddItemAsync(category2);

            for (int i = 0; i < 30; i++)
            {
                Product product = new Product();
                product.CategoryId = i % 2 == 0 ? category1.Id : category2.Id;
                product.Code = "PR" + i.ToString();
                product.Denomination = "PCS";
                product.Description = "SAMPLE PRODUCT";
                product.ImageUrl = "https://cdn.logojoy.com/wp-content/uploads/2018/05/01105857/1553.png";
                product.IsActive = true;
                product.MemberPrice = 5000 + i * 300;
                product.Name = "PRODUCT" + i.ToString();
                product.Price = 7000 + i * 300;
                await db.Products.AddItemAsync(product);
            }
        }
    }
}
