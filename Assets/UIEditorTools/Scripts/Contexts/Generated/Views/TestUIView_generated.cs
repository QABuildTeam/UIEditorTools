using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Views.Common;
using TMPro;

namespace UIEditorTools.Views
{
    public partial class TestUIView : UIView
    {

        [SerializeField]
        private Button switchToNextContextButton;
        public event Action SwitchToNextContextAction;
        private void OnSwitchToNextContextButton()
        {
            SwitchToNextContextAction?.Invoke();
        }
        public bool ActiveSwitchToNextContext
        {
            get => switchToNextContextButton.gameObject.activeInHierarchy;
            set => switchToNextContextButton.gameObject.SetActive(value);
        }
        public bool InteractiveSwitchToNextContext
        {
            get => switchToNextContextButton.interactable;
            set => switchToNextContextButton.interactable = value;
        }
        [SerializeField]
        private PreformattedTextString message;
        public string Message
        {
            set => message.Value = value;
        }

        protected override async Task Init()
        {
            switchToNextContextButton.onClick.RemoveAllListeners();
            switchToNextContextButton.onClick.AddListener(OnSwitchToNextContextButton);

        }

        protected override async Task Done()
        {
            switchToNextContextButton.onClick.RemoveAllListeners();

        }

    }
}
