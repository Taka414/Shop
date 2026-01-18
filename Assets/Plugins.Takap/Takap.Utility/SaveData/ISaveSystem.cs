// 
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// データセーブ機能を表します。
    /// </summary>
    public interface ISaveSystem
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 暗号化する際に利用するシード値を設定または取得します。
        /// </summary>
        Seed Seed { get; set; }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したオブジェクトをファイルに保存します。
        /// <para><paramref name="isBinary"/> が true の場合 暗号化バイナリ / false : JSONプレーンテキストでセーブします。</para>
        /// </summary>
        void Save<T>(string key, T src, bool isBinary = false);

        /// <summary>
        /// 指定したデータが存在するかどうか確認します。
        /// true: 存在する / false: 存在しない
        /// </summary>
        bool ExistsSaveData(string key);

        /// <summary>
        /// ファイルに保存されているデータをロードします。
        /// <para><paramref name="isBinary"/> が true の場合 暗号化バイナリ / false : JSONプレーンテキストでロードします。</para>
        /// </summary>
        T Load<T>(string name, bool isBinary = false);

        /// <summary>
        /// Json形式の平文からオブジェクトを復元します。
        /// </summary>
        T LoadFromJson<T>(string name);

        /// <summary>
        /// バイナリ形式のファイルを読み取ってオブジェクトとしてロードします。
        /// 当然アスキー形式よりこっちのほうがサイズも速度も倍くらい良好
        /// </summary>
        T LoadFromBinary<T>(string name);

        /// <summary>
        /// 指定したキーに対応するデータを削除します。
        /// </summary>
        void Delete(string name);
    }
}
