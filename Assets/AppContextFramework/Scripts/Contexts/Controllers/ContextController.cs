using ACFW.Views;
using System.Threading.Tasks;

namespace ACFW.Controllers
{
    public class ContextController : IContextController
    {
        protected IView view;

        protected bool isOpen = false;
        public bool IsOpen() => isOpen;

        public virtual Task Open()
        {
            if (isOpen)
            {
                return Task.CompletedTask;
            }
            isOpen = true;
            return view?.Show();
        }

        public virtual async Task Close()
        {
            await view?.Hide();
            isOpen = false;
            view = null;
        }

        public virtual async Task PreOpen()
        {
            if (view != null)
            {
                await view.PreShow();
            }
        }

        public virtual async Task PostOpen()
        {
            if (view != null)
            {
                await view.PostShow();
            }
        }

        public virtual async Task PreClose()
        {
            if (view != null)
            {
                await view.PreHide();
            }
        }

        public virtual async Task PostClose()
        {
            if (view != null)
            {
                await view.PostHide();
            }
        }

        public ContextController(IView view)
        {
            this.view = view;
        }
    }
}
