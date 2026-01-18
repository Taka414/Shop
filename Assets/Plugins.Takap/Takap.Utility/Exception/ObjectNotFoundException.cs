//
// (C) 2023 Takap.
//

using System;
using System.Runtime.Serialization;

namespace Takap.Utility
{
    /// <summary>
    /// (取り合えず)自作の例外の共通基底クラスを表します。
    /// </summary>
    [Serializable]
    public abstract class ApplicationException : Exception
    {
        public ApplicationException() { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception inner) : base(message, inner) { }
        protected ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// オブジェクトが存在しなかったときに発生します。
    /// </summary>
    [Serializable]
    public class ObjectNotFoundException : ApplicationException
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
