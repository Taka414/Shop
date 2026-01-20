//
// (C) 2023 Takap.
//

using System;
using System.Runtime.Serialization;

namespace Takap.Utility
{
    /// <summary>
    /// ゲーム中で発生する一般的な例外を表します。
    /// </summary>
    [Serializable]
    public class GameException : ApplicationException
    {
        public GameException() { }
        public GameException(string message) : base(message) { }
        public GameException(string message, Exception inner) : base(message, inner) { }
        protected GameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
