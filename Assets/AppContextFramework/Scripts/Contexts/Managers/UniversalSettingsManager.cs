using System.Collections.Generic;

namespace ACFW
{
    public class UniversalSettingsManager : ISettingsManager, IServiceProvider
    {
        protected Dictionary<System.Type, ISettings> settings = new Dictionary<System.Type, ISettings>();

        public UniversalSettingsManager(IEnumerable<ISettings> initialSettings)
        {
            if (initialSettings != null)
            {
                foreach (var setting in initialSettings)
                {
                    Add(setting);
                }
            }
        }

        private void Add(ISettings setting)
        {
            var type = setting.GetType();
            if (!settings.ContainsKey(type))
            {
                settings.Add(type, setting);
            }
        }

        public T Get<T>() where T : ISettings
        {
            if(settings.TryGetValue(typeof(T), out var setting))
            {
                return (T)setting;
            }
            return default;
        }
    }
}
