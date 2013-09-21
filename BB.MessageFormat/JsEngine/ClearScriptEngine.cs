using System;
using Microsoft.ClearScript.V8;

namespace BB.MessageFormat.JsEngine
{
    /// <summary>
    /// A JavaScript engine relying on ClearScript and the Google's V8 engine.
    /// </summary>
    public class ClearScriptEngine : JsEngineBase
    {
        private readonly V8ScriptEngine _engine = new V8ScriptEngine();

        private const string _compileScriptFormat = "new MessageFormat('{0}').compile('{1}')({2})";

        public ClearScriptEngine()
        {
            this._engine.Execute(this.MessageFormatJs);
            this._engine.Execute(this.LocalesJs);
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
        public override string GenerateString(string language, string formattedMessage, string data)
        {
            formattedMessage = formattedMessage.Replace(Environment.NewLine, "");
            string format = String.Format(ClearScriptEngine._compileScriptFormat, language, formattedMessage, data);
            object obj = this._engine.Evaluate(format);
            return (string) obj;
        }
    }
}