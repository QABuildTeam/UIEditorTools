namespace ACFW.Environment
{
    public class ContextEvents : IEventProvider
    {
        public UEvent<string> ActivateContext;
        public UEvent<string> OpenOverlayContext;
        public UEvent<string> CloseOverlayContext;
        public UEvent RestoreContext;
    }
}
