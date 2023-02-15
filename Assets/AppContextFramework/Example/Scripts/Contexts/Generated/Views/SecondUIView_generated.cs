using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ACFW.Views;
using TMPro;

namespace ACFW.Example.Views
{
    public partial class SecondUIView : UIView
    {

        [SerializeField]
        private Button gotoStartContextButton;
        public event Action GotoStartContextAction;
        private void OnGotoStartContextButton()
        {
            GotoStartContextAction?.Invoke();
        }
        public bool ActiveGotoStartContext
        {
            get => gotoStartContextButton.gameObject.activeInHierarchy;
            set => gotoStartContextButton.gameObject.SetActive(value);
        }
        public bool InteractiveGotoStartContext
        {
            get => gotoStartContextButton.interactable;
            set => gotoStartContextButton.interactable = value;
        }
        [SerializeField]
        private PreformattedTextString message;
        public String Message
        {
            set => message.Value = value;
        }

        [SerializeField]
        private Button openOverlayButton;
        public event Action OpenOverlayAction;
        private void OnOpenOverlayButton()
        {
            OpenOverlayAction?.Invoke();
        }
        public bool ActiveOpenOverlay
        {
            get => openOverlayButton.gameObject.activeInHierarchy;
            set => openOverlayButton.gameObject.SetActive(value);
        }
        public bool InteractiveOpenOverlay
        {
            get => openOverlayButton.interactable;
            set => openOverlayButton.interactable = value;
        }

        protected override async Task Init()
        {
            gotoStartContextButton.onClick.RemoveAllListeners();
            gotoStartContextButton.onClick.AddListener(OnGotoStartContextButton);
            openOverlayButton.onClick.RemoveAllListeners();
            openOverlayButton.onClick.AddListener(OnOpenOverlayButton);

        }

        protected override async Task Done()
        {
            gotoStartContextButton.onClick.RemoveAllListeners();
            openOverlayButton.onClick.RemoveAllListeners();

        }

    }
}
