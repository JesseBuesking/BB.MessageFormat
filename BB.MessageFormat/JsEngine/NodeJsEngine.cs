using System;
using System.Net;
using System.Threading;

namespace BB.MessageFormat.JsEngine
{
    /// <summary>
    /// A JavaScript engine relying on a nodejs server that has messageformat.js installed.
    /// </summary>
    public class NodeJsEngine : JsEngineBase
    {
        private const string _nodeJsonRequestFormat = "{{\"l\":\"{0}\",\"c\":\"{1}\", \"m\":{2}}}";

        private const int _nodeJsonRequestRetries = 3;

        private Uri NodeServer
        {
            get;
            set;
        }

        public NodeJsEngine(string nodeJsAddress)
        {
            this.NodeServer = new Uri(nodeJsAddress);
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
            var wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";

            string format = String.Format(NodeJsEngine._nodeJsonRequestFormat, language, formattedMessage, data);

            // TODO make this an exponential back-off
            int delay = 5;
            for (int i = 0; i < NodeJsEngine._nodeJsonRequestRetries; i++)
            {
                try
                {
                    return wc.UploadString(this.NodeServer, "POST", format);
                }
                catch (Exception)
                {
                    if (i == NodeJsEngine._nodeJsonRequestRetries - 1)
                        throw;

                    Thread.Sleep(delay);
                    delay = (int) (delay*2.5);
                }
            }

            return null;
        }
    }
}