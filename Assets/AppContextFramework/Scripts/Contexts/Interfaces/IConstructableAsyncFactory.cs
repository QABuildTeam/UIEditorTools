using ACFW.Views;
using System.Threading.Tasks;

namespace ACFW
{
    public interface IConstructableAsyncFactory<TResult> where TResult : IConstructable
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1> where TResult : IConstructable<T1>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2> where TResult : IConstructable<T1, T2>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3> where TResult : IConstructable<T1, T2, T3>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4> where TResult : IConstructable<T1, T2, T3, T4>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4, T5> where TResult : IConstructable<T1, T2, T3, T4, T5>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4, T5, T6> where TResult : IConstructable<T1, T2, T3, T4, T5, T6>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4, T5, T6, T7> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4, T5, T6, T7, T8> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        Task<GameObjectLoader<TResult>> Create();
    }

    public interface IConstructableAsyncFactory<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> where TResult : IConstructable<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        Task<GameObjectLoader<TResult>> Create();
    }
}
