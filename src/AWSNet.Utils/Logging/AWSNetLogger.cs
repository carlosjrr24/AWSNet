﻿using NLog;

namespace AWSNet.Utils.Logging
{
    public static class AWSNetLogger
    {
        public static void Trace(string message)
        {
            LogManager.GetCurrentClassLogger().Trace(message);
        }

        public static void Info(string message)
        {
            LogManager.GetCurrentClassLogger().Info(message);
        }

        public static void Warn(string message)
        {
            LogManager.GetCurrentClassLogger().Warn(message);
        }

        public static void Error(string message)
        {
            LogManager.GetCurrentClassLogger().Error(message);
        }

        public static void Fatal(string message)
        {
            LogManager.GetCurrentClassLogger().Fatal(message);
        }
    }
}
