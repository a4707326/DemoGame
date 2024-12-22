using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityExtern
{

    //簡化Find
    public static T Find<T>(this Transform parent, string path)
    {
        return parent.transform.Find(path).GetComponent<T>();
    }

    //在 async/await 中使用 UnityWebRequest
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest webRequest)
    {
        var taskCompletionSource = new TaskCompletionSource<UnityWebRequest>();

        webRequest.SendWebRequest().completed += operation =>
        {
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                taskCompletionSource.SetException(new UnityWebRequestException(webRequest.error));
            }
            else
            {
                taskCompletionSource.SetResult(webRequest);
            }
        };

        return taskCompletionSource.Task;
    }
    public class UnityWebRequestException : System.Exception
    {
        public UnityWebRequestException(string message) : base(message) { }
    }

}
