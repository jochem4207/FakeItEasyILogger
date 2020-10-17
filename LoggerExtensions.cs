using FakeItEasy;
using FakeItEasy.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Tests.Extensions
{
    public static class LoggerExtensions
    {
        public static void VerifyLogMustHaveHappened<T>(this ILogger<T> logger, LogLevel level, string message)
        {
            try
            {
                logger.VerifyLog(level, message)
                    .MustHaveHappened();
            }
            catch (Exception e)
            {
                throw new ExpectationException($"while verifying a call to log with message: \"{message}\"", e);
            }
        }

        public static void VerifyLogMustNotHaveHappened<T>(this ILogger<T> logger, LogLevel level, string message)
        {
            try
            {
                logger.VerifyLog(level, message)
                    .MustNotHaveHappened();
            }
            catch (Exception e)
            {
                throw new ExpectationException($"while verifying a call to log with message: \"{message}\"", e);
            }
        }

        public static IVoidArgumentValidationConfiguration VerifyLog<T>(this ILogger<T> logger, LogLevel level, string message)
        {
            return A.CallTo(logger)
                .Where(call => call.Method.Name == "Log" 
                    && call.GetArgument<LogLevel>(0) == level 
                    && CheckLogMessages(call.GetArgument<IReadOnlyList<KeyValuePair<string, object>>>(2), message));
        }

        private static bool CheckLogMessages(IReadOnlyList<KeyValuePair<string, object>> readOnlyLists, string message)
        {
            foreach(var kvp in readOnlyLists)
            {
                if (kvp.Value.ToString().Contains(message))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
