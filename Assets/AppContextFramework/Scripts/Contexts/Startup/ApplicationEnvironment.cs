using System.Collections.Generic;
using ACFW.Controllers;
using ACFW.Environment;
using ACFW.Settings;

namespace ACFW.Startup
{
    public class ApplicationEnvironment : IApplicationEnvironment
    {
        private IServiceLocator environment;
        private IStartupRunner runner;

        public ApplicationEnvironment(SettingsList settings)
        {
            environment = new UniversalEnvironment();
            environment.Add<IEventManager>(new UniversalEventManager());
            environment.Add<ISettingsManager>(new UniversalSettingsManager(settings.Settings));
        }

        public virtual void Initialize(IEnumerable<IStartupBuilder> builders, IStartupRunner runner)
        {
            foreach (var builder in builders)
            {
                builder.PopulateEnvironment(environment);
            }
            this.runner = runner;
        }

        public virtual void Run()
        {
            runner?.Run(environment);
        }
    }
}
