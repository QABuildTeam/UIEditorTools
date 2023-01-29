namespace UIEditorTools.Environment
{
    public interface IMasterCanvasManager
    {
        int ActualScreenWidth { get; }
        int ActualScreenHeight { get; }
        int ReferenceScreenWidth { get; }
        int ReferenceScreenHeight { get; }
        float ActualToReferenceWidthFactor { get; }
        float ActualToReferenceHeightFactor { get; }
        float ActualToReferenceFactor { get; }
        float ActualScreenAspect { get; }
        float ReferenceScreenAspect { get; }
    }
}
