// 
// (C) 2022 Takap.
//

using System;
using System.Text;
using System.Threading;

namespace Takap.Utility
{
    // 
    // メモ:
    // 暗号化処理のサンプル
    // 
    // StreamingAssetに配置したマップファイルとインデックスファイルからIVとキーを取得して暗号化のKeyとIVを取得する
    // マップファイルをインデックスファイルは事前にツールで生成しておく
    //   (1) Seedはゲーム開始時に初期化しておく、これ自体はメモリに平文で展開されていても問題ない
    //   (2) キーはseedから都度生成する → フィールドにキーを持たないこと
    // 

    public class EncryptionSample
    {
        Seed _seed = new Seed();
        CancellationTokenSource _cts = new CancellationTokenSource();

        public async void Sample()
        {
            // マップファイルを初期化

            // 4096バイトの0～255がランダムに書きこまれたファイル
            var a = await StreamingAssetsUtil.ReadStreamingAssetsData("a.bin", _cts.Token);
            // 4096個の0～4098までの数字がランダムに書きこまれたファイル
            var b = await StreamingAssetsUtil.ReadStreamingAssetsData("b.bin", _cts.Token);
            // 4096個の0～4098までの数字がランダムに書きこまれたファイル
            var c = await StreamingAssetsUtil.ReadStreamingAssetsData("c.bin", _cts.Token);
            _seed.Init(a, b.ToUshortArray(), c.ToUshortArray());

            // 暗号化キーの取得
            var encInfo = _seed.GetAesInfo();

            // 暗号化対象の文字列
            string message = "ああいいううええおおかかききくくけけここ";

            // 暗号化前のデータのバイナリ
            byte[] plane = Encoding.UTF8.GetBytes(message);
            ShowBytes(plane);
            // > 0xE3 0x81 0x82 0xE3 0x81 0x82 0xE3 0x81 0x84 0xE3...(60byte)

            // 文字列を暗号化
            byte[] encrypted = AesCypher.Encrypt(encInfo, plane);
            ShowBytes(encrypted);
            // > 0x90 0xD1 0xEC 0xFE 0x6D 0xD5 0x29 0x79 0x98 0xFD...(64byte)

            // 暗号化を解除
            byte[] decrypt = AesCypher.Decrypt(encInfo, encrypted); // 暗号化と同じ情報で復号化
            ShowBytes(decrypt);
            // > 0xE3 0x81 0x82 0xE3 0x81 0x82 0xE3 0x81 0x84 0xE3...(60byte)

            // 文字列に戻す
            string message2 = Encoding.UTF8.GetString(decrypt);
            Console.WriteLine(message2);
        }

        // バイト配列の内容をコンソールに表示する
        public void ShowBytes(byte[] bs)
        {
            Array.ForEach(bs, b => Console.Write($"0x{b:X2} "));
            Console.WriteLine("");
        }
    }
}
