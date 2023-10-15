using ACFW.Views;
using UnityEngine;

namespace ACFW.Controllers
{
    public class ViewLoaderFactory<TView, TReference> : IFactory<GameObjectLoader<IView>, Transform> where TView : IView where TReference : ScriptableReference<TView>
    {
        private readonly TReference _reference;
        public ViewLoaderFactory(TReference reference)
        {
            _reference = reference;
        }

        GameObjectLoader<IView> IFactory<GameObjectLoader<IView>, Transform>.Create(Transform transform)
        {
            return new GameObjectLoader<IView>(_reference.Reference, transform);
        }
    }
}
