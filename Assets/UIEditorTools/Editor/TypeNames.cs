using System;
using System.Reflection;

namespace Utility.CodeGen
{
    public static class TypeNames
    {
        public static string GetFullTypeName(Type type)
        {
            // discard `1 type suffixes
            string typeName = type.Name.Split('`')[0];
            typeName += ParseGenerics(type);
            return typeName;
        }

        public static string ParseGenerics(Type type)
        {
            string genericTypeList = "";
            System.Type[] genericArguments = type.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                genericTypeList += "<";
                string separator = "";
                foreach (System.Type genericType in genericArguments)
                {
                    genericTypeList += separator + GetFullTypeName(genericType);
                    separator = ", ";
                }
                genericTypeList += ">";
            }
            return genericTypeList;
        }

        public struct MethodArgumentStrings
        {
            public string argTypes;
            public string argList;
            public string argDeclaration;
        }
        public static MethodArgumentStrings GetMethodArgs(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            string argTypes = string.Empty;
            string argList = string.Empty;
            string argDeclaration = string.Empty;
            string separator = "";
            foreach (ParameterInfo parameter in parameters)
            {
                argTypes += separator + TypeNames.GetFullTypeName(parameter.ParameterType);
                argDeclaration += separator + TypeNames.GetFullTypeName(parameter.ParameterType) + " " + parameter.Name;
                argList += separator + parameter.Name;
                separator = ", ";
            }
            return new MethodArgumentStrings { argTypes = argTypes, argList = argList, argDeclaration = argDeclaration };
        }
    }
}
