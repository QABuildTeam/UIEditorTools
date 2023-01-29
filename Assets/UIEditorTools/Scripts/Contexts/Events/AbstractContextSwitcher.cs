using UIEditorTools.Environment;
using UnityEngine;

namespace UIEditorTools.Controllers
{
    public abstract class AbstractContextSwitcher : MonoBehaviour, IContextSwitcher
    {
        protected UniversalEnvironment environment;
        protected UniversalEventManager EventManager => environment?.Get<UniversalEventManager>();

        [SerializeField]
        protected bool disableSwitch;
        public void Init(UniversalEnvironment environment)
        {
            this.environment = environment;
            if (!disableSwitch)
            {
                Subscribe();
            }
        }

        protected abstract void Subscribe();

        public void Done()
        {
            Unsubscribe();
            environment = null;
        }

        protected abstract void Unsubscribe();
    }
}
