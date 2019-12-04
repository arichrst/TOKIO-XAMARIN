using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tokio.Services
{
    public enum ApiMethod { POST = 0 , GET = 1 }
    interface IAPIServices
    {
         Task CallApi<T>(ApiMethod method, string apiUrl,ICallback<T> callback, object body = null);
    }

    public interface ICallback<T>
    {
        void OnSuccess(T result);
        void OnFailure(string message);
        void OnStart();
        void OnEnd();
    }
}
