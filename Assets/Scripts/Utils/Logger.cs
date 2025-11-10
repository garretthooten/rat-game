using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class MyLogger
{

    private static string GetCallerName()
    {
        StackTrace stackTrace = new StackTrace(2, false);
        return stackTrace.GetFrame(0).GetMethod().DeclaringType.Name;
    }

    public static void Info(string message)
    {
        #if UNITY_EDITOR
            // string callerName = GetCallerName();
            // UnityEngine.Debug.Log($"[{callerName}] {message}");
        #endif
    }

    public static void Warning(string message)
    {
        #if UNITY_EDITOR
            // string callerName = GetCallerName();
            // UnityEngine.Debug.Log($"[{callerName}] {message}");
        #endif
    }

    public static void Error(string message)
    {
        #if UNITY_EDITOR
                // string callerName = GetCallerName();
                // UnityEngine.Debug.Log($"[{callerName}] {message}");
        #endif
    }

}
