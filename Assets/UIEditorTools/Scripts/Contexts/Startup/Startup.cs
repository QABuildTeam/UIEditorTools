using UnityEngine;
using UIEditorTools.Controllers;
using UIEditorTools.Settings;

namespace UIEditorTools.Startup
{
	public class Startup : MonoBehaviour
	{
		[SerializeField]
		protected ContextManager contextManager;
		[SerializeField]
		protected SettingsList settings;
		[SerializeField]
		protected GameContextList gameContextList;

		private static Startup instance;
		private IApplicationContext appContext;

		private void Awake()
		{
			if (instance != null)
			{
				Debug.Log($"Destroying {nameof(Startup)} duplicate");
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
			Initialize();
		}

		protected virtual IApplicationContext CreateApplicationContext()
        {
			return new ApplicationContext(contextManager, settings, gameContextList);
		}
		private void Initialize()
		{
			appContext = CreateApplicationContext();
			appContext.Initialize();
		}

		private void Start()
		{
			appContext.Run();
		}
	}
}