using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ACFW.Views;
using TMPro;

namespace ACFW.Example.Views
{
    public partial class TestOverlayUIView : UIView
    {

        [SerializeField]
        private Button closeOverlayButton;
        public event Action CloseOverlayAction;
        private void OnCloseOverlayButton()
        {
            CloseOverlayAction?.Invoke();
        }
        public bool ActiveCloseOverlay
        {
            get => closeOverlayButton.gameObject.activeInHierarchy;
            set => closeOverlayButton.gameObject.SetActive(value);
        }
        public bool InteractiveCloseOverlay
        {
            get => closeOverlayButton.interactable;
            set => closeOverlayButton.interactable = value;
        }

        protected override async Task Init()
        {
            closeOverlayButton.onClick.RemoveAllListeners();
            closeOverlayButton.onClick.AddListener(OnCloseOverlayButton);

        }

        protected override async Task Done()
        {
            closeOverlayButton.onClick.RemoveAllListeners();

        }

    }
}
