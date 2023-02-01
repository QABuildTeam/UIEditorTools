using UnityEngine;
using TMPro;

namespace Views.Common
{
    public abstract class PreformattedText : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI textValue;
        [SerializeField]
        protected string format = "{0}";
    }

    public class PreformattedText<T> : PreformattedText, IValueDisplay<T>
    {
        public T Value
        {
            set => textValue.text = string.Format(format, value);
        }
    }
}
