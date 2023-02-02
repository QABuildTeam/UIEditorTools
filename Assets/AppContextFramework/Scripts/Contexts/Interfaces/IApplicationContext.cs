namespace ACFW
{
    public interface IApplicationContext
    {
        UniversalEnvironment Environment { get; }
        void Initialize();
        void Run();
    }
}
