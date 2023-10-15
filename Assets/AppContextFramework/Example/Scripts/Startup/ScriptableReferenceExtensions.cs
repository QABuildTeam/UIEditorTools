using ACFW.Controllers;
using ACFW.Views;
using SimpleInjector;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ACFW.Example.DI
{
    public static class ScriptableReferenceExtensions
    {
        private static bool IsSuccessorOf(this Type t, Type baseType)
        {
            if (t == null || baseType == null)
            {
                return false;
            }
            if (baseType.IsInterface)
            {
                return baseType.IsAssignableFrom(t);
            }
            return t.IsSubclassOf(baseType);
        }

        private static Type GetGenericBaseInterface(this Type t, Type baseGenericInterfaceType, params Type[] genericArguments)
        {
            for (var currentType = t; currentType != null; currentType = currentType.BaseType)
            {
//                Debug.Log($"Checking base {currentType.FullName} of {t.FullName}");
                foreach (var baseInterface in currentType.GetInterfaces())
                {
//                    Debug.Log($"Checking interface {baseInterface.FullName} of type {currentType.FullName} of {t.FullName}: generic {baseInterface.IsGenericType}, generic type definition {baseInterface.IsGenericTypeDefinition}, constructed generic type {baseInterface.IsConstructedGenericType}");
                    if (baseInterface.IsGenericType && !baseInterface.IsGenericTypeDefinition && baseInterface.IsConstructedGenericType)
                    {
                        var actualGenericArguments = baseInterface.GetGenericArguments();
                        if (actualGenericArguments.Length != genericArguments.Length)
                        {
//                            Debug.Log($"actualGenericArguments.Length({actualGenericArguments.Length}) != genericArguments.Length({genericArguments.Length})");
                            continue;
                        }
                        bool argsMatch = true;
                        for (int i = 0; i < actualGenericArguments.Length; i++)
                        {
                            if (!actualGenericArguments[i].IsSuccessorOf(genericArguments[i]))
                            {
//                                Debug.Log($"{actualGenericArguments[i].FullName} != subclass({genericArguments[i].FullName})");
                                argsMatch = false;
                                break;
                            }
                        }
                        if (!argsMatch)
                        {
                            continue;
                        }
//                        Debug.Log($"{baseGenericInterfaceType.FullName}.actualGenericArguments: {string.Join(",", actualGenericArguments.Select(a => a.Name))}");
//                        Debug.Log($"{baseGenericInterfaceType.FullName}.genericArguments: {string.Join(",", genericArguments.Select(a=>a.Name))}");
                        var baseInterfaceType = baseGenericInterfaceType.MakeGenericType(actualGenericArguments);
//                        Debug.Log($"baseInterfaceType={baseInterfaceType.FullName}");
                        if (baseInterfaceType != null && baseInterfaceType.IsAssignableFrom(t))
                        {
                            return baseInterfaceType;
                        }
                    }
                }
            }
            return null;
        }

        private static Type GetGenericBaseType(this Type t, Type baseGenericType, params Type[] genericArguments)
        {
            for (var currentType = t; currentType != null; currentType = currentType.BaseType)
            {
//                Debug.Log($"Checking base {currentType.FullName} of {t.FullName}: generic {currentType.IsGenericType}, generic type definition {currentType.IsGenericTypeDefinition}, constructed generic type {currentType.IsConstructedGenericType}");
                if (currentType.IsGenericType && !currentType.IsGenericTypeDefinition && currentType.IsConstructedGenericType)
                {
                    var actualGenericArguments = currentType.GetGenericArguments();
                    if (actualGenericArguments.Length != genericArguments.Length)
                    {
//                        Debug.Log($"actualGenericArguments.Length({actualGenericArguments.Length}) != genericArguments.Length({genericArguments.Length})");
                        continue;
                    }
                    bool argsMatch = true;
                    for (int i = 0; i < actualGenericArguments.Length; ++i)
                    {
                        if (!actualGenericArguments[i].IsSuccessorOf(genericArguments[i]))
                        {
//                            Debug.Log($"{actualGenericArguments[i].FullName} != subclass({genericArguments[i].FullName})");
                            argsMatch = false;
                            break;
                        }
                    }
                    if (!argsMatch)
                    {
                        continue;
                    }
//                    Debug.Log($"{baseGenericType.FullName}.actualGenericArguments: {string.Join(",", actualGenericArguments.Select(a => a.Name))}");
//                    Debug.Log($"{baseGenericType.FullName}.genericArguments: {string.Join(",", genericArguments.Select(a => a.Name))}");
                    var baseImplementedType = baseGenericType.MakeGenericType(actualGenericArguments);
//                    Debug.Log($"baseImplementedType={baseImplementedType.FullName}");
                    if (baseImplementedType != null && t.IsSubclassOf(baseImplementedType))
                    {
                        return baseImplementedType;
                    }
                }
            }
            return null;
        }

        public static Container AddControllerAndViewFactories(this Container container)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract && (t.IsNestedPublic || t.IsPublic)).OrderBy(t => t.FullName))
            {
//                Debug.Log($"Checking type {type.FullName}");
                var viewLoaderFactoryBaseType = type.GetGenericBaseType(typeof(ViewLoaderFactory<,>), typeof(IView), typeof(ScriptableReference));
                if (viewLoaderFactoryBaseType != null)
                {
                    Debug.Log($"Registering {type.FullName} as {viewLoaderFactoryBaseType.FullName}");
                    container.Register(viewLoaderFactoryBaseType, type);
                }
                var controllerFactoryBaseType = type.GetGenericBaseInterface(typeof(IFactory<,>), typeof(IContextController), typeof(IView));
                if (controllerFactoryBaseType != null)
                {
                    Debug.Log($"Registering {type.FullName} as {controllerFactoryBaseType.FullName}");
                    container.Register(controllerFactoryBaseType, type);
                }
                var mvcContainer = type.GetGenericBaseInterface(typeof(IMVCContainer<>), typeof(ScriptableReference));
                if (mvcContainer != null)
                {
                    Debug.Log($"Registering {type.FullName} as {mvcContainer.FullName}");
                    container.Register(mvcContainer, type);
                }
                var mvcContainerFactory = type.GetGenericBaseType(typeof(AbstractMVCContainerFactory<>), typeof(ScriptableReference));
                if (mvcContainerFactory != null)
                {
                    Debug.Log($"Registering {type.FullName} as {mvcContainerFactory.FullName}");
                    container.Register(mvcContainerFactory, type);
                }
            }
            return container;
        }
    }
}
