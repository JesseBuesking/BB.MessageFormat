using BB.MessageFormat.JsEngine;
using Xunit;

namespace BB.MessageFormat.Tests.JsEngine
{
    public class NodeJsEngineTests : JsEngineTestBase
    {
        public NodeJsEngineTests()
        {
            MessageFormat.Instance.ConfigureEngine(new NodeJsEngine("http://127.0.0.1:1234/mf"));
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