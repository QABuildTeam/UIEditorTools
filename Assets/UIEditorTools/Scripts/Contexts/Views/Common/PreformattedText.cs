using UnityEngine;
using TMPro;

namespace Views.Common
{
    public abstract class PreformattedText : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI textValue;
        [SerializeField]
        protected string format;
    }

    public class PreformattedText<T> : PreformattedText
    {
        public T Value
        {
            set => textValue.text = string.Format(format, value);
        }
    }

    public class PreformattedText<T1, T2> : PreformattedText
    {
        public void Setup(T1 value1, T2 value2)
        {
            textValue.text = string.Format(format, value1, value2);
        }
    }
}
