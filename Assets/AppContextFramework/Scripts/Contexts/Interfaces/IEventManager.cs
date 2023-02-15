namespace ACFW
{
    #region UEvent generic classes definitions
    public delegate void UEvent();

    public delegate void UEvent<T1>(T1 arg1);

    public delegate void UEvent<T1, T2>(T1 arg1, T2 arg2);

    public delegate void UEvent<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    public delegate void UEvent<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate void UEvent<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    #endregion

    public interface IEventProvider { }

    public interface IEventManager
    {
        T Get<T>() where T : IEventProvider, new();
    }
}
