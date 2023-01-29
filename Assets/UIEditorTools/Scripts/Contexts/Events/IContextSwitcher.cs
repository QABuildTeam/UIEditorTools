using UIEditorTools.Environment;

namespace UIEditorTools.Controllers
{
    public interface IContextSwitcher
    {
        void Init(UniversalEnvironment environment);
        void Done();
    }
}
