using System;
using UnityEngine;
using System.Threading.Tasks;

namespace ACFW.Views
{
    public interface IGameObjectLoader<T> : IDisposable
    {
        Task<T> Load();
        GameObject LoadedObject { get; }
        T Component { get; }
    }
}
