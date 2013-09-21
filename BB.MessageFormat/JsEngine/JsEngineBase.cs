using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BB.MessageFormat.JsEngine
{
    /// <summary>
    /// Abstract base class for javascript engines.
    /// </summary>
    public abstract class JsEngineBase : IJsEngine
    {
        /// <summary>
        /// The file containing the messageformat js.
        /// </summary>
        private const string _messageFormatResource = "BB.MessageFormat.Javascript.messageformat.js";

        /// <summary>
        /// The file containing locale information.
        /// </summary>
        private const string _localeResource = "BB.MessageFormat.Javascript.locales.js";

        /// <summary>
        /// Gets the current messageformat js implementation from the embedded resource.
        /// </summary>
        protected string MessageFormatJs
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._messageFormatJs))
                    this._messageFormatJs = JsEngineBase.GetStringFromResource(JsEngineBase._messageFormatResource);

                return this._messageFormatJs;
            }
        }

        private string _messageFormatJs;

        /// <summary>
        /// Gets the current locales js from the embedded resource.
        /// </summary>
        protected string LocalesJs
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._localesJs))
                    this._localesJs = JsEngineBase.GetStringFromResource(JsEngineBase._localeResource);

                return this._localesJs;
            }
        }

        private string _localesJs;

        /// <summary>
        /// Helper method for getting resource file data.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static string GetStringFromResource(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (!assembly.GetManifestResourceNames().Contains(resource))
                throw new ArgumentException(string.Format("Requested Resource {0} Was Not Found", resource));

            var stream = assembly.GetManifestResourceStream(resource);

// ReSharper disable AssignNullToNotNullAttribute
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
// ReSharper restore AssignNullToNotNullAttribute
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
        public abstract string GenerateString(string language, string formattedMessage, string data);
    }
}