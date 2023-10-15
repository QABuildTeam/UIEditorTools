using System;
using ACFW.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ACFW.Controllers
{
    public abstract class ScriptableReference : ScriptableObject
    {
        [SerializeField]
        private AssetReference reference;
        public AssetReference Reference => reference;

        public abstract Type AssetType { get; }
    }

    public abstract class ScriptableReference<TView> : ScriptableReference where TView : IView
    {
        public override Type AssetType => typeof(TView);
    }
}
