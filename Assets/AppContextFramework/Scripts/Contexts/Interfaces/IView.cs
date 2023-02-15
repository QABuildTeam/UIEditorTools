using System.Threading.Tasks;

namespace ACFW.Views
{
    public interface IView
    {
        IServiceLocator Environment { get; set; }
        Task PreShow();
        Task Show();
        Task PostShow();
        Task PreHide();
        Task Hide();
        Task PostHide();
    }

    public interface IView<T> : IView
    {
        void Setup(T value);
    }
}
