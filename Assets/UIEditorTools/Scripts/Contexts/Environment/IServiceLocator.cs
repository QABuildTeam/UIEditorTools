namespace UIEditorTools.Environment
{
    public interface IServiceLocator
    {
        T Get<T>();
        void Add<T>(IServiceProvider provider);
    }
}
