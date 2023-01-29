using System;
using UnityEngine;
using System.Threading.Tasks;

namespace UIEditorTools.Environment
{
    public interface IObjectLoader<T> : IDisposable
    {
        Task<T> Load();
        GameObject LoadedObject { get; }
        T Component { get; }
    }
}
