using System.Collections;
using System.Collections.Generic;
using UIEditorTools.Environment;
using UIEditorTools.Controllers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UIEditorTools
{
    public abstract class ViewControllerPair : ScriptableObject
    {
        [SerializeField]
        private AssetReference viewAssetReference;
        public AssetReference ViewAssetReference => viewAssetReference;
        public abstract IContextController GetContextController(IView view, UniversalEnvironment environment);
    }

    public abstract class ViewControllerPair<TContextController, TView> : ViewControllerPair where TContextController : IContextController where TView : IView
    {
        protected abstract TContextController GetContextController(TView view, UniversalEnvironment environment);
        public override IContextController GetContextController(IView view, UniversalEnvironment environment)
        {
            return GetContextController((TView)view, environment);
        }
    }

    public abstract class ViewControllerPair<TContextController, TView, T> : ViewControllerPair where TContextController : IContextController<T> where TView : IView<T>
    {
        protected abstract TContextController GetContextController(TView view, UniversalEnvironment environment);
        public override IContextController GetContextController(IView view, UniversalEnvironment environment)
        {
            return GetContextController((TView)view, environment);
        }
    }
}
