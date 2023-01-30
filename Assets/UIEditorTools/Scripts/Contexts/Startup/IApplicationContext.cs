using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIEditorTools.Environment;

namespace UIEditorTools.Startup
{
    public interface IApplicationContext
    {
        UniversalEnvironment Environment { get; }
        void Initialize();
        void Run();
    }
}
