
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public static class DebugTools
{

    private static string debugtoolName = "[DebugTools]";
    /// <summary>
    /// 打印呼叫此方法的類別與方法名稱 "[DebugTools] Called From: Game2View.Init"
    /// </summary>
    public static void LogMethodName()
    {
        // 獲取呼叫堆疊
        StackTrace stackTrace = new StackTrace();
        // 獲取呼叫 LogMethodName 的上一層方法
        MethodBase callerMethod = stackTrace.GetFrame(1)?.GetMethod();

        if (callerMethod != null)
        {
            string className = callerMethod.DeclaringType?.Name ?? "UnknownClass";
            string methodName = callerMethod.Name;

            UnityEngine.Debug.Log($"{debugtoolName} Called From: {className}.{methodName}");
        }
        else
        {
            UnityEngine.Debug.Log($"{debugtoolName} Could not retrieve caller information.");
        }
    }

    /// <summary>
    /// 打印log複數參數並排版
    /// </summary>
    public static void Log(params object[] parameters)
    {
        Print((log) => { UnityEngine.Debug.Log(log); }, parameters);
    }
    /// <summary>
    /// 打印error複數參數並排版
    /// </summary>
    public static void LogError(params object[] parameters)
    {
        Print((log) => { UnityEngine.Debug.LogError(log); }, parameters);
    }
    /// <summary>
    /// 打印warning複數參數並排版
    /// </summary>
    public static void LogWarning(params object[] parameters)
    {
        Print((log) => { UnityEngine.Debug.LogWarning(log); }, parameters);
    }



    /// <summary>
    /// 格式化參數值
    /// </summary>
    /// <param name="value">參數值</param>
    /// <returns>格式化的字串</returns>
    private static string FormatValue(object value)
    {
        if (value == null) return "null";
        if (value is string) return $"\"{value}\""; // 字串加上雙引號
        if (value is char) return $"'{value}'"; // 字元加上單引號
        return value.ToString(); // 其他型別直接轉字串
    }


    /// <summary>
    /// 打印log複數參數並排版 [DebugTools] Parameters: param1 : "AA", param2 : 11, param3 : 'A'
    /// </summary>
    private static void Print(UnityAction<string> action,params object[] parameters)
    {

        if (parameters == null || parameters.Length == 0)
        {
            string log = $"{debugtoolName} No parameters provided.";
            action(log);
            return;
        }

        string output = $"{debugtoolName} Parameters: ";
        for (int i = 0; i < parameters.Length; i++)
        {
            string paramName = $"param{i + 1}";
            string paramValue = FormatValue(parameters[i]);
            output += $"{paramName} : {paramValue}, ";
        }

        // 去掉最後的逗號和空格
        if (output.EndsWith(", ")) output = output.Substring(0, output.Length - 2);

        action(output);
    }

}
