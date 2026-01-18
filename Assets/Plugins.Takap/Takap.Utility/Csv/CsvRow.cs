//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// CSVの1行を表します。
    /// </summary>
    public class CsvRow
    {
        /// <summary>
        /// この行の元データを取得します。
        /// </summary>
        public string[] Source { get; }

        /// <summary>
        /// この行の要素数を取得します。
        /// </summary>
        public int Count => Source.Length;

        /// <summary>
        /// 管理対象データを指定してオブジェクトを新規作成します。
        /// </summary>
        public CsvRow(string[] source)
        {
            Source = source;
        }

        /// <summary>
        /// 指定した <paramref name="n"/> 番目の要素を T の型として取得します。
        /// </summary>
        public T Get<T>(int n, Func<string, T> converter = null)
        {
            if (n > Source.Length)
            {
                throw new ArgumentOutOfRangeException($"n='{n}'は範囲外です。{this}");
            }

            return Source[n].Convert(converter);
        }

        /// <summary>
        /// <see cref="object.ToString"/> を実装します。
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Join(",", Source);
    }
}