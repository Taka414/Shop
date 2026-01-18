//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    public delegate void ActionRef<T1>(ref T1 a) where T1 : struct;
    public delegate void ActionRef<T1, T2>(ref T1 a, ref T2 b) where T1 : struct where T2 : struct;
    public delegate void ActionRef<T1, T2, T3>(ref T1 a, ref T2 b, ref T3 c) where T1 : struct where T2 : struct where T3 : struct;

    public delegate TResult FuncRef<T1, out TResult>(ref T1 a);
    public delegate TResult FuncRef<T1, T2, out TResult>(ref T1 a, ref T2 b);
    public delegate TResult FuncRef<T1, T2, T3, out TResult>(ref T1 a, ref T2 b, ref T3 c);
}
