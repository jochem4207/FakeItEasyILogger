# FakeItEasyILogger
Fake it easy Ilogger verify extension (Code in readme.md!)

.NET 5 compatible (tested with Microsoft.Extensions.Logging Version="5.0.0-rc.2.20475.5")

Based on : https://dev.azure.com/BrideButler/Bride%20Butler%20Open%20Source (sadly cannot create a PR there)

See also : https://stackoverflow.com/a/64406264/1112413 (for the history and explanation)

# Usage (copied from Bride butler)

Use
In your Test Create a fake ILogger<T>
  
`var fakeLogger = A.Fake<ILogger<T>>();`

Pass your fake logger into the class that uses the logger

`var sut = new SystemUnderTest(fakeLogger);`


After performing the actions of your tests simply call the appropriate methods on the fake logger to assert logs happened, or didn't happen based on a few criteria


```
_fakeLogger.VerifyLogMustHaveHappened(LogLevel.Error, "Failed");
_fakeLogger.VerifyLogMustNotHaveHappened(LogLevel.Error, "Failed");
```

If you want to use more of the built in FakeItEasy argument validations call the VerifyLog method

`_fakeLogger.VerifyLog(LogLevel.Error, "Failed").MustHaveHappenedTwiceOrMore();`

# Extension 
```
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
```
