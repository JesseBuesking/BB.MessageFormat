using BB.MessageFormat.JsEngine;
using Xunit;

namespace BB.MessageFormat.Tests.JsEngine
{
    public class JurassicEngineTests : JsEngineTestBase
    {
        public JurassicEngineTests()
        {
            MessageFormat.Instance.ConfigureEngine(new JurassicEngine());
        }

        public override int GetIterations()
        {
            return 100;
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