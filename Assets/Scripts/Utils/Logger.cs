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

    public static GameObject MakeDebugSphere(Vector3 position, string name = null)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.transform.position = position;

        if (name != null)
        {
            sphere.name = name;
        }
        return sphere;
    }
    

}
