using System;
using System.Diagnostics;
using UnityEngine;

public static class Debug
{
    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogFormat(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string message, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(message, args);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message.ToString());
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new Exception();
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string content)
    {
        UnityEngine.Debug.Assert(condition, content);
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params string[] args)
    {
        UnityEngine.Debug.LogWarning(string.Format(format, args));
    }

    [Conditional("NJ_ENABLE_DEBUG"), Conditional("UNITY_EDITOR")]
    public static void LogException(Exception ex)
    {
        UnityEngine.Debug.LogException(ex);
    }
}