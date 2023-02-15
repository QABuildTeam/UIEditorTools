using System.Threading.Tasks;

namespace ACFW.Views
{
    public interface IView
    {
        IServiceLocator Environment { get; set; }
        Task PreShow();
        Task Show(bool force = false);
        Task PostShow();
        Task PreHide();
        Task Hide();
        Task PostHide();
        bool HideOnOpen { get; }
    }

    public interface IView<T> : IView
    {
        void Setup(T value);
    }
}
