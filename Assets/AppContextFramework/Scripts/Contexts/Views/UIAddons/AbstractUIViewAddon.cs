using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Views
{
    public abstract class AbstractUIViewAddon : MonoBehaviour, IUIViewAddon
    {
        public virtual Task DoHideTask()
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostHideTask()
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostShowTask()
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreHideTask()
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreShowTask()
        {
            return Task.CompletedTask;
        }

        public virtual Task DoShowTask()
        {
            return Task.CompletedTask;
        }
    }
}
