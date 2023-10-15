using System.Threading.Tasks;

namespace ACFW.Views
{
    public interface IUIViewAddon
    {
        Task DoPreShowTask();
        Task DoShowTask();
        Task DoPostShowTask();
        Task DoPreHideTask();
        Task DoHideTask();
        Task DoPostHideTask();
    }
}
