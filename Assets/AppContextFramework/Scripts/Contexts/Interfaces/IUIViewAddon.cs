using System.Threading.Tasks;
using ACFW.Environment;

namespace ACFW.Views
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
