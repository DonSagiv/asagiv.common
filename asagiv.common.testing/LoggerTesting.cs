using asagiv.common.Logging;

namespace asagiv.common.testing
{
    public class LoggerTesting
    {
        [Fact]
        public void AssertLoggerNotNull()
        {
            var logger = LoggerFactory.CreateLogger();

            Assert.NotNull(logger);
        }

        [Fact]
        public void AssertNoLogError()
        {
            var exception = Record.Exception(() =>
            {
                var logger = LoggerFactory.CreateLogger();

                logger.Information("This is just a test log.");
            });

            Assert.Null(exception);
        }
    }
}