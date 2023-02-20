namespace ACFW.Startup
{
    public interface IStartupRunner
    {
        void Run(IServiceLocator environment);
    }
}
