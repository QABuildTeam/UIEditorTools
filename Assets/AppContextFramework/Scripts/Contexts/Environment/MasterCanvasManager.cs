using UnityEngine;
using UnityEngine.UI;

namespace ACFW.Views
{
    public class MasterCanvasManager : MonoBehaviour, IMasterCanvasManager, IServiceProvider
    {
        [SerializeField]
        private CanvasScaler masterCanvasScaler;
        [SerializeField]
        private Vector2 defaultReferenceResolutiuon;

        public int ActualScreenWidth => Screen.width;

        public int ActualScreenHeight => Screen.height;

        public int ReferenceScreenWidth => (int)(masterCanvasScaler != null ? masterCanvasScaler.referenceResolution.x : defaultReferenceResolutiuon.x);

        public int ReferenceScreenHeight => (int)(masterCanvasScaler != null ? masterCanvasScaler.referenceResolution.y : defaultReferenceResolutiuon.y);

        public float ActualToReferenceWidthFactor => (float)ActualScreenWidth / (float)ReferenceScreenWidth;

        public float ActualToReferenceHeightFactor => (float)ActualScreenHeight / (float)ReferenceScreenHeight;

        public float ActualToReferenceFactor => Mathf.Min(ActualToReferenceWidthFactor, ActualToReferenceHeightFactor);

        public float ActualScreenAspect => (float)ActualScreenWidth / (float)ActualScreenHeight;

        public float ReferenceScreenAspect => (float)ReferenceScreenWidth / (float)ReferenceScreenHeight;

        public void Init()
        {
            if (ActualScreenAspect < ReferenceScreenAspect)
            {
                masterCanvasScaler.matchWidthOrHeight = 1;
            }
            else
            {
                masterCanvasScaler.matchWidthOrHeight = 0;
            }
        }
    }
}
