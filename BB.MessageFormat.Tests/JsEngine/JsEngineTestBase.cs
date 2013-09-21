using System;
using System.Diagnostics;
using Xunit;

namespace BB.MessageFormat.Tests.JsEngine
{
    public abstract class JsEngineTestBase
    {
        private const string _compileMessage1 = @"
{GENDER, select,
    male {He}
  female {She}
   other {They}
} found {NUM_RESULTS, plural,
            one {1 result}
          other {# results}
        } in {NUM_CATEGORIES, plural,
                  one {1 category}
                other {# categories}
             }.";

        private const string _compileMessage2 = @"You {NUM_ADDS, plural, offset:1
              =0{didnt add this to your profile}
              zero{added this to your profile}
              one{and one other person added this to their profile}
              other{and # others added this to their profiles}
          }.";

        public abstract int GetIterations();

        public abstract int GetWarmup();

        public void AddToProfileMessageTestBase()
        {
            string generateString = MessageFormat.Instance.GenerateString(
                "en", _compileMessage2, @"{""NUM_ADDS"":0}");
            Assert.Equal("You didnt add this to your profile.", generateString);
        }

        public void ResultsInCategoriesMessageTestBase()
        {
            string generateString = MessageFormat.Instance.GenerateString("en", _compileMessage1, @"{
  ""GENDER""         : ""male"",
  ""NUM_RESULTS""    : 1,
  ""NUM_CATEGORIES"" : 2
}");
            Assert.Equal("He found 1 result in 2 categories.", generateString);
        }

        public void PerformanceBase()
        {
            int iterations = this.GetIterations();
            int warmup = this.GetWarmup();

            var results = new string[iterations];
            for (int i = 0; i < warmup; i++)
                results[i] = MessageFormat.Instance.GenerateString("en", _compileMessage2, @"{""NUM_ADDS"":0}");

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
                results[i] = MessageFormat.Instance.GenerateString("en", _compileMessage2, @"{""NUM_ADDS"":0}");

            for (int i = 0; i < iterations; i++)
                Assert.Equal("You didnt add this to your profile.", results[i]);

            long elapse = sw.ElapsedMilliseconds;

            Console.WriteLine("{0:#,##0.0#} ops/ms", (float) iterations/elapse);
            Console.WriteLine("{0:#,##0.0#} ops/s", iterations/(elapse/1000.0));
        }
    }
}