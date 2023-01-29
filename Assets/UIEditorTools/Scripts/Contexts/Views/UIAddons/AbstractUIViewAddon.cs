using UIEditorTools.Environment;
using System.Threading.Tasks;
using UnityEngine;

namespace UIEditorTools.Views
{
    public abstract class AbstractUIViewAddon : MonoBehaviour, IUIViewAddon
    {
        public virtual Task DoHideTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostHideTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPostShowTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreHideTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoPreShowTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }

        public virtual Task DoShowTask(UniversalEnvironment environment)
        {
            return Task.CompletedTask;
        }
    }
}
