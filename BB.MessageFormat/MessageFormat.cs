using System;
using System.Diagnostics;
using System.Threading;
using BB.MessageFormat.JsEngine;

namespace BB.MessageFormat
{
    /// <summary>
    /// Using messageformat.js as the underlying formatting engine, this singleton will compile your formatted message
    /// and generate a string given data.
    /// </summary>
    public class MessageFormat : IJsEngine
    {
        private static readonly Lazy<MessageFormat> _lazy = new Lazy<MessageFormat>(
            () => new MessageFormat(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static MessageFormat Instance
        {
            get { return MessageFormat._lazy.Value; }
        }

        private IJsEngine Engine
        {
            get;
            set;
        }

        internal AccessorDelegate MemoryAccessor
        {
            get;
            set;
        }

        internal SetterDelegate MemorySetter
        {
            get;
            set;
        }

        internal AccessorDelegate CacheAccessor
        {
            get;
            set;
        }

        internal SetterDelegate CacheSetter
        {
            get;
            set;
        }

        internal AccessorDelegate DatabaseAccessor
        {
            get;
            set;
        }

        internal SetterDelegate DatabaseSetter
        {
            get;
            set;
        }

        internal Func<string> LanguageAccessor
        {
            get;
            set;
        }

        private MessageFormat()
        {
        }

        public delegate bool AccessorDelegate(string language, string key, out string value);

        public delegate bool SetterDelegate(string language, string key, string value);

        /// <summary>
        /// Configures the JavaScript engine that should be used to perform the string generation.
        /// </summary>
        /// <param name="engine"></param>
        public void ConfigureEngine(IJsEngine engine)
        {
            Debug.Assert(null != engine);
            this.Engine = engine;
        }

        /// <summary>
        /// Configures additional information.
        /// </summary>
        /// <param name="memoryAccessor"></param>
        /// <param name="memorySetter"></param>
        /// <param name="cacheAccessor"></param>
        /// <param name="cacheSetter"></param>
        /// <param name="databaseAccessor"></param>
        /// <param name="databaseSetter"></param>
        /// <param name="languageAccessor"></param>
        public void ConfigureOther(AccessorDelegate memoryAccessor, SetterDelegate memorySetter,
            AccessorDelegate cacheAccessor, SetterDelegate cacheSetter, AccessorDelegate databaseAccessor,
            SetterDelegate databaseSetter, Func<string> languageAccessor)
        {
            Debug.Assert(null != memoryAccessor);
            Debug.Assert(null != memorySetter);
            Debug.Assert(null != cacheAccessor);
            Debug.Assert(null != cacheSetter);
            Debug.Assert(null != databaseAccessor);
            Debug.Assert(null != databaseSetter);
            Debug.Assert(null != languageAccessor);

            this.MemoryAccessor = memoryAccessor;
            this.MemorySetter = memorySetter;
            this.CacheAccessor = cacheAccessor;
            this.CacheSetter = cacheSetter;
            this.DatabaseAccessor = databaseAccessor;
            this.DatabaseSetter = databaseSetter;
            this.LanguageAccessor = languageAccessor;
        }

        /// <summary>
        /// Creates a string using messageformat.js.
        /// </summary>
        /// <param name="language">
        /// The language to use.
        /// <example>
        /// <para>
        /// language: en-EN OR en-US OR es-ES
        /// </para>
        /// </example>
        /// </param>
        /// <param name="formattedMessage">
        /// A messageformat formatted message.
        /// <example>
        /// <para>
        /// formattedMessage: {GENDER, select, male {He} female {She} other {They}} found it!
        /// </para>
        /// </example>
        /// </param>
        /// <param name="data">
        /// The data used to create the final string.
        /// <example>
        /// <para>
        /// data: {"GENDER": "male"}
        /// </para>
        /// </example>
        /// </param>
        /// <returns></returns>
        public string GenerateString(string language, string formattedMessage, string data)
        {
            return this.Engine.GenerateString(language, formattedMessage, data);
        }
    }
}