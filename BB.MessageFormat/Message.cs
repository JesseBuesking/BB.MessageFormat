using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BB.MessageFormat
{
    /// <summary>
    /// A messageformat-able message object.
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// The Id of the current message.
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// The default value for the message.
        /// </summary>
        private readonly string _default;

        /// <summary>
        /// Is this a messageformat string, or is it a simple string that doesn't required any compilation / generation.
        /// </summary>
        public readonly bool IsMessageFormat;

        /// <summary>
        /// Cached hashcode for faster lookup.
        /// </summary>
        private readonly int _hashCode;

        /// <summary>
        /// Constructor for a message.
        /// </summary>
        /// <param name="id">The unique id for this message.</param>
        /// <param name="defaultValue">The default value to return for this message.</param>
        /// <param name="isMessageFormat">
        /// If this is actually a messageformat.js message. (if it's not, we can short-circuit the messageformat
        /// compiling logic and return the value as-is - plus it allows us to perform some runtime checks)
        /// </param>
        public Message(int id, string defaultValue, bool isMessageFormat = false)
        {
            this._id = id;
            this._default = defaultValue;
            this.IsMessageFormat = isMessageFormat;

            int hash = 13;
            hash = (hash*7) + this._id.GetHashCode();
            hash = (hash*7) + this._default.GetHashCode();
            hash = (hash*7) + this.IsMessageFormat.GetHashCode();
            this._hashCode = hash;
        }

        public static implicit operator string(Message message)
        {
            if (message.IsMessageFormat)
                throw new MissingMessageFormatDataException("need to supply messageformat data");
            return Resolver(message, null);
        }

        /// <summary>
        /// Resolves the message.
        /// </summary>
        /// <param name="messageFormatData">The data to be passed to the compiled messageformat message.</param>
        /// <returns></returns>
        public string Resolve(string messageFormatData)
        {
            return Resolver(this, messageFormatData);
        }

        /// <summary>
        /// Resolves the message.
        /// </summary>
        /// <param name="messageFormatData">The data to be passed to the compiled messageformat message.</param>
        /// <returns></returns>
        public string Resolve(object messageFormatData)
        {
            string format = Message.GenerateDynamicString(messageFormatData);
            return Resolver(this, format);
        }

        private static string Resolver(Message message, string formattedMessage)
        {
            string language = MessageFormat.Instance.LanguageAccessor();
            return ResolverInternal(message, language, formattedMessage)
                ?? ResolverInternal(message, "en-EN", formattedMessage);
        }

        /// <summary>
        /// 1. Get the formatted message for the language supplied from one of our sources (memory, cache, db, default).
        /// 2. Generate the final string.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="language"></param>
        /// <param name="formattedMessage"></param>
        /// <returns></returns>
        private static string ResolverInternal(Message message, string language, string formattedMessage)
        {
            string valueId = message._id.ToString(CultureInfo.InvariantCulture);
            string result;
            if (MessageFormat.Instance.MemoryAccessor(language, valueId, out result))
            {
                return (null != formattedMessage)
                    ? MessageFormat.Instance.GenerateString(language, result, formattedMessage)
                    : result;
            }

            if (MessageFormat.Instance.CacheAccessor(language, valueId, out result))
            {
                MessageFormat.Instance.MemorySetter(language, valueId, result);

                return (null != formattedMessage)
                    ? MessageFormat.Instance.GenerateString(language, result, formattedMessage)
                    : result;
            }

            if (MessageFormat.Instance.DatabaseAccessor(language, valueId, out result))
            {
                MessageFormat.Instance.MemorySetter(language, valueId, result);
                MessageFormat.Instance.CacheSetter(language, valueId, result);

                return (null != formattedMessage)
                    ? MessageFormat.Instance.GenerateString(language, result, formattedMessage)
                    : result;
            }

            return (null != formattedMessage)
                ? MessageFormat.Instance.GenerateString(language, message._default, formattedMessage)
                : message._default;
        }

        /// <summary>
        /// Creates a string from the object supplied.
        /// <example>
        /// Given new {Data="blah"}, this would create the string '{"Data":"blah"}'.
        /// </example>
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private static string GenerateDynamicString(object @params)
        {
            if (null == @params)
                return null;

            PropertyInfo[] props = @params.GetType().GetProperties();

            StringBuilder sb = new StringBuilder("{");
            foreach (var prop in props.OrderBy(p => p.Name))
                sb.AppendFormat("\"{0}\":\"{1}\",", prop.Name, prop.GetValue(@params, null));

            return sb.ToString(0, sb.Length - 1) + "}";
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string value = this;
            return value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (!(obj is Message))
                return false;

            var objMessage = (Message) obj;
            return (this._id == objMessage._id)
                && (this._default == objMessage._default)
                && (this.IsMessageFormat == objMessage.IsMessageFormat);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this._hashCode;
        }
    }
}