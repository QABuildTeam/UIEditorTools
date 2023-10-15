using ACFW.Views;

namespace ACFW.Controllers
{
    public abstract class ContextControllerFactory<TReference, TView> : IFactory<IContextController, IView> where TReference : ScriptableReference<TView> where TView : IView
    {
        public abstract IContextController Create(IView view);
    }
}
