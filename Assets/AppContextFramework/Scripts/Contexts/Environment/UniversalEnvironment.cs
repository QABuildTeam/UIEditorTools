using System.Collections.Generic;

namespace ACFW
{
    public class UniversalEnvironment : IServiceLocator
    {
        private Dictionary<System.Type, IServiceProvider> providers = new Dictionary<System.Type, IServiceProvider>();

        public T Get<T>()
        {
            System.Type type = typeof(T);
            if (providers.ContainsKey(type))
            {
                return (T)providers[type];
            }
            return default;
        }

        public void Add<T>(IServiceProvider provider)
        {
            System.Type type = typeof(T);
            if (!providers.ContainsKey(type))
            {
                providers.Add(type, provider);
            }
        }
    }
}
