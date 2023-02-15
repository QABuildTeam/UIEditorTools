using ACFW.Views;
using System.Threading.Tasks;

namespace ACFW.Controllers
{
    public class ContextController : IContextController
    {
        protected IView view;

        protected bool isOpen = false;
        public bool IsOpen() => isOpen;
        protected IServiceLocator environment;

        public virtual Task Open()
        {
            if (isOpen)
            {
                return Task.CompletedTask;
            }
            isOpen = true;
            if (view != null)
            {
                view.Environment = environment;
            }
            return view?.Show();
        }

        public virtual async Task Close()
        {
            await view?.Hide();
            isOpen = false;
            view = null;
            environment = null;
        }

        public virtual async Task PreOpen()
        {
            if (view != null)
            {
                view.Environment = environment;
                await view.PreShow();
            }
        }

        public virtual async Task PostOpen()
        {
            if (view != null)
            {
                view.Environment = environment;
                await view.PostShow();
            }
        }

        public virtual async Task PreClose()
        {
            if (view != null)
            {
                view.Environment = environment;
                await view.PreHide();
            }
        }

        public virtual async Task PostClose()
        {
            if (view != null)
            {
                view.Environment = environment;
                await view.PostHide();
            }
        }

        public ContextController(IView view, IServiceLocator environment)
        {
            this.view = view;
            this.environment = environment;
        }
    }
}
