using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UIEditorTools.Environment;

namespace UIEditorTools
{
    public interface IView
    {
        UniversalEnvironment Environment { get; set; }
        Task PreShow();
        Task Show(bool force = false);
        Task PostShow();
        Task PreHide();
        Task Hide();
        Task PostHide();
        bool HideOnOpen { get; }
    }

    public interface IView<T> : IView
    {
        void Setup(T value);
    }
}
