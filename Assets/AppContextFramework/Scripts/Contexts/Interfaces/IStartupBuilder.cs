namespace ACFW.Startup
{
    public interface IStartupBuilder
    {
        void PopulateEnvironment(IServiceLocator environment);
    }
}
