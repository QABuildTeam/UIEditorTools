using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInjector;
using UnityEngine.Video;
using ACFW.Views;
using ACFW.Example.Environment;
using ACFW.Controllers;
using SimpleAudioSystem;
using DIAudioSystem;
using SimpleAudioSystem.Environment;
using ACFW.Environment;
using ACFW.Example.Controllers;
using System.Linq;
using ACFW.Example.DI;

namespace ACFW.Example
{
    public class DIAppStartup : MonoBehaviour
    {
        [SerializeField]
        private MasterCanvasManager masterCanvasManager;
        [SerializeField]
        private WorldTransformManager worldTransformManager;
        [SerializeField]
        private UITransformManager uiTransformManager;
        [SerializeField]
        private ScriptableObject[] settings;
        [SerializeField]
        private AppContext[] appContexts;

        private Container container;
        private void Awake()
        {
            container = new Container();
            Debug.Log($"Registeering instances");
            // GameObject instances
            container.RegisterInstance<MasterCanvasManager>(masterCanvasManager);
            container.RegisterInitializer<MasterCanvasManager>((masterCanvas) => masterCanvas.Init());
            container.RegisterInstance<WorldTransformManager>(worldTransformManager);
            container.RegisterInstance<UITransformManager>(uiTransformManager);

            Debug.Log("Registering settings");
            // settings
            foreach (var setting in settings)
            {
                container.RegisterInstance(setting.GetType(), setting);
            }

            Debug.Log("Registering app contexts");
            // app contexts
            foreach (var appContext in appContexts)
            {
                container.Collection.AppendInstance<AppContext>(appContext);
            }

            Debug.Log("Registering events");
            // events
            container.RegisterSingleton<TestEvents>();
            container.RegisterSingleton<SecondEvents>();
            container.RegisterSingleton<TestOverlayEvents>();

            Debug.Log("Registering audio subsystem");
            // audio
            container.RegisterSingleton<AudioEvents>();
            container.Register<IAudioPlayer, DIAudioSystem.SimpleAudioPlayer>();
            container.Register<IAudioEventsHandler, DIAudioSystem.AudioEventsHandler>();
            container.RegisterSingleton<DIAudioSystem.AudioManager>();

            Debug.Log("Registering context switchers collection");
            // switchers
            container.Collection.Register<IContextSwitcher>(new[]
            {
                typeof(TestSwitcher),
                typeof(SecondSwitcher),
                typeof(TestOverlaySwitcher)
            }, Lifestyle.Singleton);

            Debug.Log("Registering startup objects");
            // startup
            container.Register<ContextEvents>(Lifestyle.Singleton);
            container.Register<AppContextSelector>(Lifestyle.Singleton);
            container.Register<ContextManager>(Lifestyle.Singleton);

            Debug.Log("Registering controller and view factories");
            container.AddControllerAndViewFactories();

            Debug.Log("Verifying DI container");
            container.Verify();
        }

        private ContextManager contextManager;
        private void Start()
        {
            contextManager = container.GetInstance<ContextManager>();
            var contextEvents = container.GetInstance<ContextEvents>();
            var appContextCollection = container.GetAllInstances<AppContext>().ToList();
            var firstAppContext = appContextCollection.First();
            contextEvents.ActivateContext?.Invoke(firstAppContext.Id);
        }
    }
}
