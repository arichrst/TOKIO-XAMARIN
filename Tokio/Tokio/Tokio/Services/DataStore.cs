using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Tokio.Models;
using System.Collections.Generic;
using System.IO;
using Firebase.Storage;

namespace Tokio.Services
{
    public class DataStore<T> : IDataStore<T>
    {
        private string TableName;
        FirebaseClient firebase = new FirebaseClient("https://tokio-961ae.firebaseio.com/");
        FirebaseStorage storage = new FirebaseStorage("tokio-961ae.appspot.com");

        public DataStore()
        {
            TableName = typeof(T).Name;
            if (TableName != "Store" & TableName != "User")
                TableName = App.MyStore == null? typeof(T).Name : typeof(T).Name  + App.MyStore.Id.ToString();
            TableName = TableName.Replace("-", "");
        }

        public async Task<bool> AddItemAsync(T item)
        {
            try
            {
                await firebase.Child(TableName).PostAsync(item);
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteItemAsync(long id)
        {
            try
            {
                var data = (await firebase.Child(TableName).OnceAsync<T>())
                    .Where(a => a.Object.GetType().GetProperty("Id").GetValue(a.Object).ToString() == id.ToString())
                    .FirstOrDefault();
                await firebase.Child(TableName).Child(data.Key).DeleteAsync();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<T> GetItemAsync(long id, bool loadFromCache = false)
        {
            T result = default(T);
            try
            {
                if (loadFromCache)
                    result = new List<T>().Load("LIST" + TableName).FirstOrDefault(x => x.GetType().GetProperty("Id").GetValue(x).ToString() == id.ToString());
            }
            catch { }
            try
            {
                if (result == null)
                {
                    return (await firebase.Child(TableName).OnceAsync<T>())
                        .Where(a => a.Object.GetType().GetProperty("Id").GetValue(a.Object).ToString() == id.ToString())
                        .FirstOrDefault().Object;
                }
                return result;
            }
            catch
            {
                return await Task.FromResult(default(T));
            }
        }

        public async Task<System.Collections.Generic.IEnumerable<T>> GetItemsAsync(Func<T, bool> expression = null , bool loadFromCache = false)
        {
            IEnumerable<T> result = null;
            if (loadFromCache)
            {
                try
                {
                    result = new List<T>().Load("LIST" + TableName);
                    if (result == null)
                        result = new List<T>();
                }
                catch { }
            }
            if (result == null || result.Count() == 0)
            {
                result = (await firebase
                      .Child(TableName)
                      .OnceAsync<T>()).Select(item => item.Object);
                if (result == null)
                    result = new List<T>();
                result.Save("LIST" + TableName);
                if (expression == null)
                {
                    return result;
                }
                else
                {
                    return result.Where(expression);
                }
            }
            if (expression == null)
            {
                return result;
            }
            else
            {
                return result.Where(expression);
            }
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            try
            {
                string id = item.GetType().GetProperty("Id").GetValue(item, null).ToString();
                var data = (await firebase.Child(TableName).OnceAsync<T>())
                    .Where(a => a.Object.GetType().GetProperty("Id").GetValue(a.Object).ToString() == id)
                    .FirstOrDefault();
                await firebase.Child(TableName).Child(data.Key).PutAsync(item);
                return await Task.FromResult<bool>(true);
            }
            catch
            {
                return await Task.FromResult<bool>(false);
            }
        }

        public async Task<string> Upload(Stream stream, long id)
        {
            return await storage.Child(TableName).Child(id.ToString() + ".png").PutAsync(stream);
        }
    }
}
