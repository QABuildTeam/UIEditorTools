using UnityEngine;

namespace UIEditorTools.Environment
{
    public class WorldTransformManager : MonoBehaviour, ITransformManager
    {
        [SerializeField]
        private Transform workingTransform;
        public Transform InitialTransform => workingTransform;

        public Transform WorkingTransform => workingTransform;
    }
}
