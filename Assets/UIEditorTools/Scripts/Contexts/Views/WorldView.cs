using UIEditorTools.Environment;
using System.Threading.Tasks;
using UnityEngine;

namespace UIEditorTools.Views
{
    public class WorldView : MonoBehaviour, IView
    {
        public UniversalEnvironment Environment { get; set; }

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

        public virtual Task Show(bool force = false)
        {
            gameObject.SetActive(true);
            return Task.CompletedTask;
        }
    }
}
