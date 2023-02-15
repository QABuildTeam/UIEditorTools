using System.Threading.Tasks;

namespace ACFW.Views
{
    public interface IUIViewAddon
    {
        Task DoPreShowTask(IServiceLocator environment);
        Task DoShowTask(IServiceLocator environment);
        Task DoPostShowTask(IServiceLocator environment);
        Task DoPreHideTask(IServiceLocator environment);
        Task DoHideTask(IServiceLocator environment);
        Task DoPostHideTask(IServiceLocator environment);
    }
}
