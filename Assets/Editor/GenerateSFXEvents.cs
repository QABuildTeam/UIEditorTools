using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using ACFW;

namespace SimpleAudioSystem.Editor
{
    public class GenerateSFXEvents : EditorWindow
    {
        private const string sfxEventsFolder = "Scripts/Globals/Audio/SFXContexts";

        private const string body = @"// This file has been automatically generated
// by GenerateSFXEvents
// Don't edit it
// Instead, use menu item Tools/Generate SFX event classes

namespace {2}
{{
    public class {0} : ContextSFXStarter
    {{
        private void Awake()
        {{
            {1}
        }}
    }}
}}
";

        private const string eventManagerHandlerTemplate = "EventManager.Get<{0}>().{1} += StartSFX;";
        private const string iLocalHubHandlerTemplate = "LocalHub.{1} += StartSFX;";

        private static void GenerateClass(string eventDomain, string eventName, string template, string eventNamespace)
        {
            string className = eventDomain + "_" + eventName + "_SFXStarter";
            string sfxEventDomainFolder = Path.Combine(Application.dataPath, sfxEventsFolder, eventDomain);
            string sfxEventClass = Path.Combine(sfxEventDomainFolder, className + ".cs");
            string eventManagerHandler = string.Format(template, eventDomain, eventName);
            string eventManagerBody = string.Format(body, className, eventManagerHandler, eventNamespace);

            Directory.CreateDirectory(sfxEventDomainFolder);
            using (StreamWriter stream = new StreamWriter(sfxEventClass))
            {
                stream.Write(eventManagerBody);
            }
        }

        //[MenuItem("Tools/Generate SFX event classes")]
        private static void Generate()
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in a.GetTypes())
                {
                    UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Full type name is {type.FullName}");
                    var fullNameParts = type.FullName.Split('.');
                    var typeNamespace = string.Join(".", fullNameParts.Take(fullNameParts.Length - 1));
                    if (typeof(IEventProvider).IsAssignableFrom(type))
                    {
                        UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Type {type.Name} implements {typeof(IEventProvider).Name}");
                        foreach (FieldInfo uevent in type.GetFields())
                        {
                            // only AudioBinding events get generated classes
                            if (uevent.GetCustomAttributes(true).Any(attr => attr is AudioBindingAttribute))
                            {
                                Type ueventType = uevent.FieldType;
                                //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} has type {ueventType.Name}");
                                if (!ueventType.IsGenericType)
                                {
                                    if (uevent.FieldType == typeof(UEvent))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} is of type {typeof(UEvent)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                }
                                else
                                {
                                    Type ueventGenericType = ueventType.GetGenericTypeDefinition();
                                    //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} has generic type definition {uevent.FieldType.GetGenericTypeDefinition()}");
                                    if (ueventGenericType == typeof(UEvent<>))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} is of type {typeof(uevent<>)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                    else if (ueventGenericType == typeof(UEvent<,>))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{UEvent.Name} is of type {typeof(UEvent<,>)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                    else if (ueventGenericType == typeof(UEvent<,,>))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} is of type {typeof(UEvent<,,>)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                    else if (ueventGenericType == typeof(UEvent<,,,>))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} is of type {typeof(UEvent<,,,>)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                    else if (ueventGenericType == typeof(UEvent<,,,,>))
                                    {
                                        //UnityEngine.Debug.Log($"[GenerateSFXEvents.Generate] Field {type.Name}.{uevent.Name} is of type {typeof(UEvent<,,,>)}");
                                        GenerateClass(type.Name, uevent.Name, eventManagerHandlerTemplate, type.Namespace);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }
    }
}
