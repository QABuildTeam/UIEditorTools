using ACFW.Views;
using UnityEngine;

namespace ACFW.Controllers
{
    public interface IMVCContainer
    {
        public bool IsUI { get; }
        IFactory<GameObjectLoader<IView>, Transform> ViewLoaderFactory { get; }
        IFactory<IContextController, IView> ControllerFactory { get; }
    }

    public interface IMVCContainer<TReference> : IMVCContainer where TReference : ScriptableReference { }
    public abstract class AbstractMVCContainerFactory<TReference> : IFactory<IMVCContainer> where TReference : ScriptableReference
    {
        public abstract IMVCContainer Create();
    }
}
