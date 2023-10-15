using System;

namespace ACFW
{
    public interface IConstructable : IDisposable
    {
        void Construct();
    }

    public interface IConstructable<T1> : IDisposable
    {
        void Construct(T1 arg1);
    }

    public interface IConstructable<T1, T2> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2);
    }

    public interface IConstructable<T1, T2, T3> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IConstructable<T1, T2, T3, T4> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }

    public interface IConstructable<T1, T2, T3, T4, T5> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }

    public interface IConstructable<T1, T2, T3, T4, T5, T6> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    }

    public interface IConstructable<T1, T2, T3, T4, T5, T6, T7> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    }

    public interface IConstructable<T1, T2, T3, T4, T5, T6, T7, T8> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
    }

    public interface IConstructable<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IDisposable
    {
        void Construct(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
    }
}
