using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Views
{
    public abstract class AbstractUIViewAddon : MonoBehaviour, IUIViewAddon
    {
        public virtual Task DoHideTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostHideTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostShowTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreHideTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreShowTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoShowTask(IServiceLocator environment)
        {
            return Task.CompletedTask;
        }
    }
}
