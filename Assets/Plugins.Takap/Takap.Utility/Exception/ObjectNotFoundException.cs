//
// (C) 2023 Takap.
//

using System;
using System.Runtime.Serialization;

namespace Takap.Utility
{

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
