//
// (C) 2026 Takap.
//

namespace System.Runtime.CompilerServices
{
    // コンパイラが init キーワードを処理するために必要なダミークラス
    // 本来は .NET 5 以降に含まれているが、Unity の環境にはないので自前で定義する
    public static class IsExternalInit
    {
    }
}