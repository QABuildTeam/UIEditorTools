using ACFW.Environment;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Views
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasViewInitializer : AbstractUIViewAddon
    {
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private int initialSortingOrder = 1;
        [SerializeField]
        private int workingSortingOrder = 0;
        [SerializeField]
        private CanvasGroup canvasGroup;
        public override Task DoPreShowTask()
        {
            canvas.sortingOrder = initialSortingOrder;
            canvasGroup.alpha = 0;
            return base.DoPreShowTask();
        }

        public override Task DoPostShowTask()
        {
            canvas.sortingOrder = workingSortingOrder;
            canvasGroup.alpha = 1;
            return base.DoPostShowTask();
        }

        public override Task DoPreHideTask()
        {
            canvas.sortingOrder = workingSortingOrder;
            return base.DoPreHideTask();
        }
    }
}
