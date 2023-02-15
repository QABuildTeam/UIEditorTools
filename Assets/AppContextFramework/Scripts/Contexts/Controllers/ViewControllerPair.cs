using ACFW.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ACFW.Controllers
{
    public abstract class ViewControllerPair : ScriptableObject
    {
        [SerializeField]
        private AssetReference viewAssetReference;
        public AssetReference ViewAssetReference => viewAssetReference;
        public abstract IContextController GetContextController(IView view, IServiceLocator environment);
    }

    public abstract class ViewControllerPair<TContextController, TView> : ViewControllerPair where TContextController : IContextController where TView : IView
    {
        protected abstract TContextController GetContextController(TView view, IServiceLocator environment);
        public override IContextController GetContextController(IView view, IServiceLocator environment)
        {
            return GetContextController((TView)view, environment);
        }
    }

    public abstract class ViewControllerPair<TContextController, TView, T> : ViewControllerPair where TContextController : IContextController<T> where TView : IView<T>
    {
        protected abstract TContextController GetContextController(TView view, IServiceLocator environment);
        public override IContextController GetContextController(IView view, IServiceLocator environment)
        {
            return GetContextController((TView)view, environment);
        }
    }
}
