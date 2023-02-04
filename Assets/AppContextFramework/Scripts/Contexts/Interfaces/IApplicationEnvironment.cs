namespace ACFW
{
    public interface IApplicationEnvironment
    {
        UniversalEnvironment Environment { get; }
        void Initialize();
        void Run();
    }
}
