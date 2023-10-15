namespace ACFW
{
    public interface IConstructableFactory<TResult> where TResult : IConstructable
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1> where TResult : IConstructable<T1>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2> where TResult : IConstructable<T1, T2>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3> where TResult : IConstructable<T1, T2, T3>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4> where TResult : IConstructable<T1, T2, T3, T4>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4, T5> where TResult : IConstructable<T1, T2, T3, T4, T5>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4, T5, T6> where TResult : IConstructable<T1, T2, T3, T4, T5, T6>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4, T5, T6, T7> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4, T5, T6, T7, T8> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        TResult Create();
    }

    public interface IConstructableFactory<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        TResult Create();
    }
}
