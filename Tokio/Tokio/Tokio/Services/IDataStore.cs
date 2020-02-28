using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tokio.Models;

namespace Tokio.Services
{
    public interface IDataStore<T> 
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(long id);
        Task<T> GetItemAsync(long id, bool loadFromCache = false);
        Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> expression = null , bool loadFromCache = false);
        Task<string> Upload(Stream stream, long id);
    }
}
