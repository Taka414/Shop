//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Updateを自分で管理するための管理システムで発生したエラー情報を格納します。
    /// </summary>
    public class LocalUpdateSystemErrorInfo
    {
        /// <summary>
        /// エラーが発生したオブジェクトを取得します。
        /// </summary>
        public IUpdatable SourceObject { get; private set; }

        /// <summary>
        /// 発生した例外を取得します。
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 発生した状況を指定してオブジェクトを新規作成します。
        /// </summary>
        public LocalUpdateSystemErrorInfo(IUpdatable source, Exception ex)
        {
            SourceObject = source;
            Exception = ex;
        }
    }
}
