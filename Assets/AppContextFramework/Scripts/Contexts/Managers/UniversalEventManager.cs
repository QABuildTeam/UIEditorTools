using System.Collections.Generic;

namespace ACFW.Controllers
{
    public class UniversalEventManager : IEventManager, IServiceProvider
    {
        protected Dictionary<System.Type, IEventProvider> eventHub = new Dictionary<System.Type, IEventProvider>();

        public T Get<T>() where T : IEventProvider, new()
        {
            System.Type type = typeof(T);
            if (eventHub.ContainsKey(type))
            {
                return (T)eventHub[type];
            }
            else
            {
                T eventProvider = new T();
                eventHub.Add(type, eventProvider);
                return eventProvider;
            }
        }

    }
}
