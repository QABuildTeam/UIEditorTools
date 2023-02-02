using ACFW.Environment;

namespace ACFW
{
    public interface IContextSwitcher
    {
        void Init(UniversalEnvironment environment);
        void Done();
    }
}
