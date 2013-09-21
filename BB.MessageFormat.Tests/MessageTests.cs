using BB.MessageFormat.JsEngine;
using Xunit;

namespace BB.MessageFormat.Tests
{
    public class MessageTests
    {
        private static bool GenericAccessor(string language, string key, out string result)
        {
            switch (language)
            {
                case "en-EN":
                    {
                        switch (key)
                        {
                            case "0":
                                result = "Hello";
                                return true;
                            case "1":
                                result = "GoodBye";
                                return true;
                            case "2":
                                result = @"{GENDER, select, male {He} female {She} other {They}} wins!";
                                return true;
                            default:
                                result = null;
                                return false;
                        }
                    }
                case "es-ES":
                    {
                        switch (key)
                        {
                            case "0":
                                result = "Bienvenida";
                                return true;
                            case "1":
                                result = "Despedida";
                                return true;
                            case "2":
                                result = @"{GENDER, select, male {El} female {Ella} other {El}} gana!";
                                return true;
                            default:
                                result = null;
                                return false;
                        }
                    }
                default:
                    result = null;
                    return false;
            }
        }

        private static bool GenericSetter(string language, string key, string value)
        {
            return true;
        }

        private static string EnglishLanguage()
        {
            return "en-EN";
        }

        private static string SpanishLanguage()
        {
            return "es-ES";
        }

        public MessageTests()
        {
            MessageFormat.Instance.ConfigureEngine(new ClearScriptEngine());
        }

        [Fact]
        public void EnglishHello()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                EnglishLanguage
                );

            var hello = new Message(0, "hello");

            // Should use the type coming from the memory accessor.
            Assert.Equal("Hello", hello);
        }

        [Fact]
        public void EnglishGoodBye()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                EnglishLanguage
                );

            var hello = new Message(1, "goodbye");

            // Should use the type coming from the memory accessor.
            Assert.Equal("GoodBye", hello);
        }

        [Fact]
        public void EnglishMessageFormatted()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                EnglishLanguage
                );

            var hello = new Message(2, "ignoreme", true);

            Assert.Throws<MissingMessageFormatDataException>(() =>
                {
// ReSharper disable UnusedVariable
                    string getFinalValue = hello;
// ReSharper restore UnusedVariable
                });

            // Should use the type coming from the memory accessor.

            Assert.Equal("He wins!", hello.Resolve(@"{""GENDER"":""male""}"));
            Assert.Equal("He wins!", hello.Resolve(new {GENDER = "male"}));

            Assert.Equal("She wins!", hello.Resolve(@"{""GENDER"":""female""}"));
            Assert.Equal("She wins!", hello.Resolve(new {GENDER = "female"}));

            Assert.Equal("They wins!", hello.Resolve(@"{""GENDER"":""sasquatch""}"));
            Assert.Equal("They wins!", hello.Resolve(new {GENDER = "sasquatch"}));
        }

        [Fact]
        public void EnglishOther()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                EnglishLanguage
                );

            var hello = new Message(4, "Other");

            // Should use the default type.
            Assert.Equal("Other", hello);
        }

        [Fact]
        public void SpanishHello()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                SpanishLanguage
                );

            var hello = new Message(0, "hello");

            // Should use the type coming from the memory accessor.
            Assert.Equal("Bienvenida", hello);
        }

        [Fact]
        public void SpanishGoodBye()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                SpanishLanguage
                );

            var hello = new Message(1, "goodbye");

            // Should use the type coming from the memory accessor.
            Assert.Equal("Despedida", hello);
        }

        [Fact]
        public void SpanishMessageFormatted()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                SpanishLanguage
                );

            var hello = new Message(2, "ignoreme", true);

            Assert.Throws<MissingMessageFormatDataException>(() =>
                {
// ReSharper disable UnusedVariable
                    string getFinalValue = hello;
// ReSharper restore UnusedVariable
                });

            // Should use the type coming from the memory accessor.

            Assert.Equal("El gana!", hello.Resolve(@"{""GENDER"":""male""}"));
            Assert.Equal("El gana!", hello.Resolve(new {GENDER = "male"}));

            Assert.Equal("Ella gana!", hello.Resolve(@"{""GENDER"":""female""}"));
            Assert.Equal("Ella gana!", hello.Resolve(new {GENDER = "female"}));

            Assert.Equal("El gana!", hello.Resolve(@"{""GENDER"":""sasquatch""}"));
            Assert.Equal("El gana!", hello.Resolve(new {GENDER = "sasquatch"}));
        }

        [Fact]
        public void SpanishOther()
        {
            MessageFormat.Instance.ConfigureOther(
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                GenericAccessor,
                GenericSetter,
                SpanishLanguage
                );

            var hello = new Message(4, "Other");

            // Should use the default type.
            Assert.Equal("Other", hello);
        }
    }
}