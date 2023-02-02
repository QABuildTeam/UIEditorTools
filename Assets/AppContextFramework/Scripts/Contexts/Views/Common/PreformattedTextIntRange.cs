namespace ACFW.Views
{
    public struct IntRange
    {
        public int value;
        public int max;
        public override string ToString()
        {
            return $"{value}/{max}";
        }
    }
    public class PreformattedTextIntRange : PreformattedText<IntRange>
    {
    }
}
