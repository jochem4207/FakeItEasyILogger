# FakeItEasyILogger
Fake it easy Ilogger verify extension 

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


`
_fakeLogger.VerifyLogMustHaveHappened(LogLevel.Error, "Failed");

_fakeLogger.VerifyLogMustNotHaveHappened(LogLevel.Error, "Failed");
`

If you want to use more of the built in FakeItEasy argument validations call the VerifyLog method

`_fakeLogger.VerifyLog(LogLevel.Error, "Failed").MustHaveHappenedTwiceOrMore();`
