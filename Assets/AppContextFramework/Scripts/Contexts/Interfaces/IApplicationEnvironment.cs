using System.Collections.Generic;

namespace ACFW.Startup
{
    public interface IApplicationEnvironment
    {
        void Initialize(IEnumerable<IStartupBuilder> builders, IStartupRunner runner);
        void Run();
    }
}
