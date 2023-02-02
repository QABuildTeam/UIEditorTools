namespace ACFW
{
    public interface IServiceLocator
    {
        T Get<T>();
        void Add<T>(IServiceProvider provider);
    }
}
