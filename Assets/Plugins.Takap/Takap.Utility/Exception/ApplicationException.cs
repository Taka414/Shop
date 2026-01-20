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
}
