using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UIEditorTools;
using UIEditorTools.Views;
using UIEditorTools.Environment;
using UIEditorTools.Controllers;
using Views.Common;
using TMPro;

namespace UIEditorTools.Views
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

        protected override async Task Init()
        {
            gotoStartContextButton.onClick.RemoveAllListeners();
            gotoStartContextButton.onClick.AddListener(OnGotoStartContextButton);

        }

        protected override async Task Done()
        {
            gotoStartContextButton.onClick.RemoveAllListeners();

        }

    }
}
