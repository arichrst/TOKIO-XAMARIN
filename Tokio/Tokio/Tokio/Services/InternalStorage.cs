using System;
using System.IO;
using Newtonsoft.Json;
using Tokio.Models;
namespace Tokio.Services
{
    public static class InternalStorage
    {
        private static readonly string FILE_EXTENSION = ".data";
        public static void Save<T>(this T data, string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename + FILE_EXTENSION);
            if (!File.Exists(path)) {
                var fs = new FileStream(path, FileMode.OpenOrCreate);
                fs.Dispose();
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }

        public static T Load<T>(this T data, string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename + FILE_EXTENSION);
            if (!File.Exists(path))
            {
                var fs = new FileStream(path, FileMode.Create);
                fs.Dispose();
            }
            try
            {
                string cache = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(cache);
            }
            catch (Exception) { return default(T); }
        }

        public static void Remove(string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename + FILE_EXTENSION);
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {

            }
        }

        public static void SaveAsProfile(this User data)
        {
            data.Save("PROFILE");
        }

        public static User LoadProfile(this User data)
        {
            return data.Load("PROFILE");
        }

        public static void RemoveProfile(this User data)
        {
            Remove("PROFILE");
        }
    }
}
