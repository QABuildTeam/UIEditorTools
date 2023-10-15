using ACFW.Views;
using UnityEngine;

namespace ACFW.Controllers
{
    public abstract class AbstractMVCContainer<TReference> : IMVCContainer<TReference> where TReference : ScriptableReference
    {
        private readonly IFactory<GameObjectLoader<IView>, Transform> _viewLoaderFactory;
        private readonly IFactory<IContextController, IView> _contextControllerFactory;
        public AbstractMVCContainer(IFactory<GameObjectLoader<IView>, Transform> viewLoaderFactory, IFactory<IContextController, IView> contextControllerFactory)
        {
            _viewLoaderFactory = viewLoaderFactory;
            _contextControllerFactory = contextControllerFactory;
        }

        public IFactory<GameObjectLoader<IView>, Transform> ViewLoaderFactory => _viewLoaderFactory;

        public IFactory<IContextController, IView> ControllerFactory => _contextControllerFactory;

        public abstract bool IsUI { get; }
    }
}
