namespace BB.MessageFormat.JsEngine
{
    /// <summary>
    /// Interface for JavaScript engines.
    /// </summary>
    public interface IJsEngine
    {
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
        string GenerateString(string language, string formattedMessage, string data);
    }
}