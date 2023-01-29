using UnityEngine;
using UIEditorTools.Controllers;
using UIEditorTools.Settings;

namespace UIEditorTools.Startup
{
	public class Startup : MonoBehaviour
	{
		[SerializeField]
		private ContextManager contextManager;
		[SerializeField]
		private SettingsList settings;
		[SerializeField]
		private GameContextList gameContextList;

		private static Startup instance;
		private ApplicationContext appContext;

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

		private void Initialize()
		{
			appContext = new ApplicationContext(contextManager, settings, gameContextList);
			appContext.Initialize();
		}

		private void Start()
		{
			appContext.Run();
		}
	}
}