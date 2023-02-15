namespace ACFW
{
    public interface IContextSwitcher
    {
        void Init(IServiceLocator environment);
        void Done();
    }
}
