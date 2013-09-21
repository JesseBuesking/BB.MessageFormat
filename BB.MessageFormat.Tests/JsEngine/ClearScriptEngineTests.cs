using BB.MessageFormat.JsEngine;
using Xunit;

namespace BB.MessageFormat.Tests.JsEngine
{
    public class ClearScriptEngineTests : JsEngineTestBase
    {
        public ClearScriptEngineTests()
        {
            MessageFormat.Instance.ConfigureEngine(new ClearScriptEngine());
        }

        public override int GetIterations()
        {
            return 10000;
        }

        public override int GetWarmup()
        {
            return 10;
        }

        [Fact]
        public void AddToProfileMessageTest()
        {
            this.AddToProfileMessageTestBase();
        }

        [Fact]
        public void ResultsInCategoriesMessageTest()
        {
            this.ResultsInCategoriesMessageTestBase();
        }

        [Fact]
        public void Performance()
        {
            this.PerformanceBase();
        }
    }
}