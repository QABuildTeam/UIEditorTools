using UnityEngine;

namespace ACFW.Views
{
    public interface IValueDisplay<T>
    {
        public T Value { set; }
    }

    public interface IValueDisplay<T1, T2>
    {
        public void Setup(T1 value1, T2 value2);
    }
}
