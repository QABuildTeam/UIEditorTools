using SimpleInjector;
using System.Collections.Generic;
using ACFW.Controllers;

namespace ACFW.Example.DI
{
    public static class AppContextExtensions
    {
        public static Container AddAppContexts(this Container container, IEnumerable<AppContext> appContexts)
        {
            foreach (AppContext appContext in appContexts)
            {
                foreach (var worldObject in appContext.worldObjects)
                {
                    container.Register(worldObject.GetType());
                }
                foreach (var uiObject in appContext.uiObjects)
                {
                    container.Register(uiObject.GetType());
                }
            }
            return container;
        }
    }
}
