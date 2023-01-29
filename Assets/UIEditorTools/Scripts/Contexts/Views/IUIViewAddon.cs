using System.Threading.Tasks;
using UIEditorTools.Environment;

namespace UIEditorTools.Views
{
    public interface IUIViewAddon
    {
        Task DoPreShowTask(UniversalEnvironment environment);
        Task DoShowTask(UniversalEnvironment environment);
        Task DoPostShowTask(UniversalEnvironment environment);
        Task DoPreHideTask(UniversalEnvironment environment);
        Task DoHideTask(UniversalEnvironment environment);
        Task DoPostHideTask(UniversalEnvironment environment);
    }
}
