using UnityEngine;

namespace ACFW.Environment
{
    public abstract class ServiceProvider : MonoBehaviour, IServiceProvider
    {
        [SerializeField]
        private bool disabled = false;
        public bool Disabled => disabled;

        public abstract void Register(IServiceLocator locator);
    }
}
