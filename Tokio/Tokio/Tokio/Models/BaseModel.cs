using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tokio.Services;

namespace Tokio.Models
{
    public class BaseModel
    {
        public long Id { get; set; }
        public BaseModel()
        {
            Id = DateTime.Now.Ticks;
        }
    }

    public static class BaseModelExtension
    {
        public static async Task<bool> SaveToDB<T>(this T data) where T : BaseModel
        {
            return await new DataStore<T>().AddItemAsync(data);
        }

        public static async Task<T> FromDB<T>(this T data, long id)where T : BaseModel
        {
            return await new DataStore<T>().GetItemAsync(id);
        }

        public static async Task<IEnumerable<T>> FromDB<T>(this IEnumerable<T> data , Func<T,bool> expression = null) where T : BaseModel
        {
            return await new DataStore<T>().GetItemsAsync(expression);
        }
    }
}
