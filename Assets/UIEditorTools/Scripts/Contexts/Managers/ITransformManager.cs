using UnityEngine;

namespace UIEditorTools
{
    public interface ITransformManager
    {
        /// <summary>
        /// A `beyond the screen' transform where an object should initially be instantiated
        /// </summary>
        Transform InitialTransform { get; }
        /// <summary>
        /// An `inside the screen' transform where the object should be moved to in order to be visible
        /// </summary>
        Transform WorkingTransform { get; }
    }
}
