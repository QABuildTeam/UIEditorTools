using System.Collections.Generic;
using UIEditorTools.Environment;
using UnityEngine;

namespace UIEditorTools
{
    public class UniversalSettingsManager : IServiceProvider
    {
        protected Dictionary<System.Type, ScriptableObject> settings = new Dictionary<System.Type, ScriptableObject>();

        public UniversalSettingsManager(IEnumerable<ScriptableObject> initialSettings)
        {
            if (initialSettings != null)
            {
                foreach (var setting in initialSettings)
                {
                    Add(setting);
                }
            }
        }

        private void Add(ScriptableObject setting)
        {
            var type = setting.GetType();
            if (!settings.ContainsKey(type))
            {
                settings.Add(type, setting);
            }
        }

        public T Get<T>() where T : ScriptableObject
        {
            if(settings.TryGetValue(typeof(T), out var setting))
            {
                return (T)setting;
            }
            return default;
        }
    }
}
