using ACFW.Environment;
using UnityEngine;

namespace ACFW.Controllers
{
    public abstract class AbstractContextSwitcher : MonoBehaviour, IContextSwitcher
    {
        protected IServiceLocator environment;
        protected IEventManager EventManager => environment?.Get<IEventManager>();

        [SerializeField]
        protected bool disableSwitch;
        public void Init(IServiceLocator environment)
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
