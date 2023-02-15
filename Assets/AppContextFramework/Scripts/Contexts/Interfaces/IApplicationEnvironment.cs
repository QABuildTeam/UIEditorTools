namespace ACFW
{
    public interface IApplicationEnvironment
    {
        IServiceLocator Environment { get; }
        void Initialize();
        void Run();
    }
}
