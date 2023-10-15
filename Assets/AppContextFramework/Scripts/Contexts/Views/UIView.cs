using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Views
{
    public class UIView : MonoBehaviour, IView
    {
        protected IUIViewAddon[] addons;
        protected IUIViewAddon[] Addons => addons == null ? (addons = GetComponents<IUIViewAddon>()) : addons;

        public IServiceLocator Environment { get; set; }

        protected virtual Task Init()
        {
            return Task.CompletedTask;
        }

        protected virtual Task Done()
        {
            return Task.CompletedTask;
        }

        protected bool isVisible = false;

        public virtual async Task Hide()
        {
            if (!isVisible || !gameObject.activeInHierarchy)
            {
                return;
            }
            isVisible = false;
            if (Addons != null)
            {
                await Task.WhenAll(Addons.Select(a => a.DoHideTask()));
            }
            await Done();
            Environment = null;
            gameObject.SetActive(false);
        }

        public virtual async Task Show()
        {
            if (isVisible)
            {
                return;
            }
            gameObject.SetActive(true);
            await Init();
            if (Addons != null)
            {
                await Task.WhenAll(Addons.Select(a => a.DoShowTask()));
            }
            isVisible = true;
        }

        public virtual async Task PreShow()
        {
            if (isVisible)
            {
                return;
            }
            if (Addons != null)
            {
                gameObject.SetActive(true);
                await Task.WhenAll(Addons.Select(a => a.DoPreShowTask()));
                gameObject.SetActive(false);
            }
        }

        public virtual async Task PostShow()
        {
            if (!isVisible || !gameObject.activeInHierarchy)
            {
                return;
            }
            if (Addons != null)
            {
                await Task.WhenAll(Addons.Select(a => a.DoPostShowTask()));
            }
        }

        public virtual async Task PreHide()
        {
            if (!isVisible || !gameObject.activeInHierarchy)
            {
                return;
            }
            if (Addons != null)
            {
                await Task.WhenAll(Addons.Select(a => a.DoPreHideTask()));
            }
        }

        public virtual async Task PostHide()
        {
            if (isVisible)
            {
                return;
            }
            if (Addons != null)
            {
                gameObject.SetActive(true);
                await Task.WhenAll(Addons.Select(a => a.DoPostHideTask()));
                gameObject.SetActive(false);
            }
        } 
    }
}
