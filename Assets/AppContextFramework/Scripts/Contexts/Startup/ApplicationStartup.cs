using UnityEngine;
using ACFW.Controllers;
using ACFW.Settings;

namespace ACFW.Startup
{
	public class ApplicationStartup : MonoBehaviour
	{
		[SerializeField]
		protected SettingsList settings;

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

		private void Initialize()
		{
			appEnvironment = new ApplicationEnvironment(settings);
			var builders = GetComponentsInChildren<IStartupBuilder>();
			var runner = GetComponentInChildren<IStartupRunner>();
			appEnvironment.Initialize(builders, runner);
		}

		private void Start()
		{
			appEnvironment.Run();
		}
	}
}