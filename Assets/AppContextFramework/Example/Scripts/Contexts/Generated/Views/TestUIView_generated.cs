using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ACFW.Views;
using TMPro;

namespace ACFW.Example.Views
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
        public String Message
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
