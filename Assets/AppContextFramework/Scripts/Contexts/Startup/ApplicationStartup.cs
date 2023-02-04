using UnityEngine;
using ACFW.Controllers;
using ACFW.Settings;

namespace ACFW.Startup
{
	public class ApplicationStartup : MonoBehaviour
	{
		[SerializeField]
		protected ContextManager contextManager;
		[SerializeField]
		protected SettingsList settings;
		[SerializeField]
		protected AppContextList appContextList;

		private static ApplicationStartup instance;
		private IApplicationEnvironment appEnvironment;

		private void Awake()
		{
			if (instance != null)
			{
				Debug.Log($"Destroying {nameof(ApplicationStartup)} duplicate");
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
			Initialize();
		}

		protected virtual IApplicationEnvironment CreateApplicationEnvironment()
        {
			return new ApplicationEnvironment(contextManager, settings, appContextList);
		}
		private void Initialize()
		{
			appEnvironment = CreateApplicationEnvironment();
			appEnvironment.Initialize();
		}

		private void Start()
		{
			appEnvironment.Run();
		}
	}
}