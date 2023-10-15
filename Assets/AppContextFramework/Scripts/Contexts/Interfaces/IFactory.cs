namespace ACFW
{
    public interface IFactory<TResult> where TResult : class
    {
        TResult Create();
    }

    public interface IFactory<TResult, T1> where TResult : class
    {
        TResult Create(T1 arg1);
    }

    public interface IFactory<TResult, T1, T2> where TResult : class
    {
        TResult Create(T1 arg1, T2 arg2);
    }
}
