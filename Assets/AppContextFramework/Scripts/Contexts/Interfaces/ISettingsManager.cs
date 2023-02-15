using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACFW
{
    public interface ISettingsManager
    {
        T Get<T>() where T : ISettings;
    }
}
