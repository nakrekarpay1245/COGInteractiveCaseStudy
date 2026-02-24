using System.Diagnostics;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
using Debug = UnityEngine.Debug;
#endif

namespace _Game.Utilities
{
    public enum RichLogLevel
    {
        Log,
        Warning,
        Error
    }

    public static class RichLogger
    {
        private static readonly StringBuilder _sb = new StringBuilder(256);

        public static class Color
        {
            public const string red = "red";
            public const string green = "green";
            public const string blue = "blue";
            public const string yellow = "yellow";
            public const string orange = "orange";
            public const string purple = "purple";
            public const string cyan = "cyan";
            public const string white = "white";
            public const string black = "black";
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message, string color = null, Object context = null, bool callerInfo = true)
        {
            LogInternal(RichLogLevel.Log, message, context, callerInfo, color);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message, string color = "yellow", Object context = null, bool callerInfo = true)
        {
            LogInternal(RichLogLevel.Warning, message, context, callerInfo, color);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message, string color = "red", Object context = null, bool callerInfo = true)
        {
            LogInternal(RichLogLevel.Error, message, context, callerInfo, color);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogInternal(RichLogLevel level, object message, Object context, bool callerInfo, string color)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _sb.Clear();

            if (callerInfo)
            {
                var stackTrace = new StackTrace(2, true);
                var frame = stackTrace.GetFrame(0);
                var method = frame?.GetMethod();

                if (method?.DeclaringType != null)
                {
                    _sb.Append('[')
                      .Append(method.DeclaringType.Name)
                      .Append('.')
                      .Append(method.Name)
                      .Append(':')
                      .Append(frame.GetFileLineNumber())
                      .Append("] ");
                }
            }

            if (context != null)
            {
                _sb.Append('[').Append(context.name).Append("] ");
            }

            string msg = message.ToString();
            
            if (!string.IsNullOrEmpty(color))
                _sb.Append($"<color={color}>{msg}</color>");
            else
                _sb.Append(msg);

            string output = _sb.ToString();

            switch (level)
            {
                case RichLogLevel.Log: Debug.Log(output, context); break;
                case RichLogLevel.Warning: Debug.LogWarning(output, context); break;
                case RichLogLevel.Error: Debug.LogError(output, context); break;
            }
#endif
        }
    }
}