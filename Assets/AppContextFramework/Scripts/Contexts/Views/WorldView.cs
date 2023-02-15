using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Views
{
    public class WorldView : MonoBehaviour, IView
    {
        public IServiceLocator Environment { get; set; }

        public bool HideOnOpen => false;

        public virtual Task Hide()
        {
            gameObject.SetActive(false);
            return Task.CompletedTask;
        }

        public virtual Task PostHide()
        {
            return Task.CompletedTask;
        }

        public virtual Task PostShow()
        {
            return Task.CompletedTask;
        }

        public virtual Task PreHide()
        {
            return Task.CompletedTask;
        }

        public virtual Task PreShow()
        {
            return Task.CompletedTask;
        }

        public virtual Task Show()
        {
            gameObject.SetActive(true);
            return Task.CompletedTask;
        }
    }
}
