using ElasticSearch.Loader.Model;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ElasticSearch.Loader
{
    internal static class AssemblyLoader
    {
        public static MainModel LoadAssembly(string dllPath, string xmlPath)
        {
            MainModel mainModel = new MainModel();

            Assembly assembly = Assembly.LoadFrom(dllPath);

            mainModel.AssemblyFullName = assembly.GetName().Name;

            mainModel.ClassNodeList = LoadClass(assembly, xmlPath);// string exePath

            return mainModel;
        }

        private static List<ClassNode> LoadClass(Assembly assembly, string xmlPath)
        {
            //Load XML
            var xmlDoc = LoadXml(xmlPath);
            if (xmlDoc != null)
            {
                if (xmlDoc.Members != null && xmlDoc.Members.Length > 0)
                {
                    foreach (var member in xmlDoc.Members)
                    {
                        if (!string.IsNullOrEmpty(member.Summary))
                        {
                            member.Summary = member.Summary.Trim();
                        }
                    }
                }
            }

            //LoadTypes
            var types = assembly.GetTypes();

            List<ClassNode> items;
            if (xmlDoc != null)
            {
                items = LoadClasses(types, xmlDoc.Members);
            }
            else
            {
                items = LoadClasses(types, null);
            }

            return items;
        }

        static XmlDoc LoadXml(string xmlPath)
        {
            XmlDoc doc = null;

            if (!string.IsNullOrEmpty(xmlPath))
            {
                var element = XElement.Load(xmlPath);

                doc = LoadXml(element);
            }

            return doc;
        }

        static XmlDoc LoadXml(XElement document)
        {
            XmlDoc returnValue = new XmlDoc();

            if (document != null)
            {
                var assemblyElement = document.Element("assembly");
                if (assemblyElement != null)
                {
                    returnValue.Assembly = new XmlAssembly();
                    var nameElement = assemblyElement.Element("name");
                    if (nameElement != null)
                    {
                        returnValue.Assembly.Name = nameElement.Value;
                    }
                }

                var members = document.Element("members");
                if (members != null)
                {
                    var xmlMembers = new List<XmlMember>();

                    var items = members.Elements();
                    foreach (var member in items)
                    {
                        XmlMember xmlMember = new XmlMember();

                        //Name
                        var nameAttribute = member.Attribute("name");
                        if (nameAttribute != null)
                        {
                            xmlMember.Name = nameAttribute.Value;
                        }

                        //Summary
                        var summaryElement = member.Element("summary");
                        if (summaryElement != null)
                        {
                            if (!string.IsNullOrEmpty(summaryElement.Value))
                            {
                                xmlMember.Summary = summaryElement.Value.Trim();
                            }
                        }

                        var paramElements = member.Elements("param");
                        if (paramElements != null)
                        {
                            var xmlMemberParamList = new List<XmlMemberParam>();
                            foreach (var paramElement in paramElements)
                            {
                                var paramNameAttribute = paramElement.Attribute("name");
                                var paramValueElement = paramElement.Value;
                                if (paramNameAttribute != null && !string.IsNullOrEmpty(paramNameAttribute.Value) && !string.IsNullOrEmpty(paramValueElement))
                                {
                                    var xmlMemberParam = new XmlMemberParam();

                                    xmlMemberParam.Name = paramNameAttribute.Value;
                                    xmlMemberParam.Value = paramValueElement;

                                    xmlMemberParamList.Add(xmlMemberParam);
                                }
                            }
                            if (xmlMemberParamList.Count > 0)
                            {
                                xmlMember.Param = xmlMemberParamList.ToArray();
                            }
                        }

                        var returns = member.Element("returns");
                        if (returns != null)
                        {
                            if (!string.IsNullOrEmpty(returns.Value))
                            {
                                xmlMember.Returns = returns.Value;
                            }
                        }

                        xmlMembers.Add(xmlMember);
                    }

                    returnValue.Members = xmlMembers.ToArray();
                }
            }

            return returnValue;
        }

        private static List<ClassNode> LoadClasses(Type[] classes, XmlMember[] xmlMembers)
        {
            List<ClassNode> returnValue = new List<ClassNode>();

            foreach (var item in classes)
            {
                var classNode = new ClassNode();

                classNode.Name = item.Name;
                classNode.FullName = item.FullName;
                classNode.Namespace = item.Namespace;
                classNode.IsAbstract = item.IsAbstract;

                var classMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => string.Format("T:{0}", item.FullName).Equals(v.Name)) : null;
                if (classMember != null)
                {
                    classNode.Summary = classMember.Summary;
                }

                //BaseType
                if (item.BaseType != null && item.BaseType != typeof(object))
                {
                    classNode.BaseTypeName = item.BaseType.Name;
                    classNode.BaseTypeFullName = item.BaseType.FullName;
                }

                List<Type> includes = new List<Type>();//baiji
                List<string> imports = new List<string>();//java

                //Properties
                var properties = item.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var propertyNodes = LoadProperties(properties, xmlMembers, includes, imports);
                if (propertyNodes != null && propertyNodes.Count > 0)
                {
                    foreach (var propertyNode in propertyNodes)
                    {
                        classNode.PropertyNodeList.Add(propertyNode);
                    }
                }

                //Methods
                var methods = item.GetMembers()
                    .Where(v => v.MemberType == MemberTypes.Method)
                    .Where(v => v.DeclaringType == item)
                    .Where(v => !v.Name.StartsWith("StartIOCPTaskOf"))
                    .Select(v => (MethodInfo)v)
                    .Where(v => v.IsPublic && !v.IsSpecialName)
                    .ToArray();
                var methodNodes = LoadMethods(methods, xmlMembers, includes);
                if (methodNodes != null && methodNodes.Count > 0)
                {
                    foreach (var methodNode in methodNodes)
                    {
                        classNode.MethodNodeList.Add(methodNode);
                    }
                }

                classNode.Includes = includes.Distinct().OrderBy(v => v.FullName).ToList();
                classNode.Imports = imports.Distinct().OrderBy(v => v).ToList();

                returnValue.Add(classNode);
            }

            return returnValue;
        }

        private static List<PropertyNode> LoadProperties(PropertyInfo[] properties, XmlMember[] xmlMembers, List<Type> includes, List<string> imports)
        {
            List<PropertyNode> returnValue = new List<PropertyNode>();

            if (properties != null && properties.Length > 0)
            {
                foreach (var property in properties)
                {
                    var propertyNode = new PropertyNode();

                    propertyNode.Name = property.Name;
                    propertyNode.PropertyType = property.PropertyType;
                    propertyNode.FieldAttribute = property.GetCustomAttribute<FieldAttribute>(false);

                    var propertyMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => string.Format("P:{0}.{1}", property.DeclaringType.FullName, property.Name).Equals(v.Name)) : null;
                    if (propertyMember != null)
                    {
                        propertyNode.Summary = propertyMember.Summary;
                    }

                    returnValue.Add(propertyNode);
                }
            }

            return returnValue;
        }

        private static List<MethodNode> LoadMethods(MethodInfo[] methods, XmlMember[] xmlMembers, List<Type> includes)
        {
            List<MethodNode> returnValue = new List<MethodNode>();

            if (methods != null && methods.Length > 0)
            {
                foreach (var method in methods)
                {
                    var methodModel = new MethodNode();

                    methodModel.Name = method.Name;

                    switch (method.Name)
                    {
                        default:
                            {
                                methodModel.ReturnType = method.ReturnType;
                                var parameters = method.GetParameters();
                                if (parameters.Length > 0)
                                {
                                    methodModel.Parameters = new List<MethodParameter>();
                                    foreach (var parameter in parameters)
                                    {
                                        var methodParameter = new MethodParameter();

                                        methodParameter.Name = parameter.Name;
                                        methodParameter.ParameterType = parameter.ParameterType;

                                        methodModel.Parameters.Add(methodParameter);
                                    }
                                }

                                var xmlMemberName = string.Format("M:{0}.{1}", method.DeclaringType.FullName, method.Name);
                                if (methodModel.Parameters != null && methodModel.Parameters.Count > 0)
                                {
                                    xmlMemberName = string.Concat(xmlMemberName, "(", string.Join(",", methodModel.Parameters.Select(v => v.ParameterType.FullName)), ")");
                                }
                                var methodMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => xmlMemberName.Equals(v.Name)) : null;
                                if (methodMember != null)
                                {
                                    methodModel.Summary = methodMember.Summary;
                                }
                            }
                            break;
                    }

                    returnValue.Add(methodModel);
                }
            }

            return returnValue;
        }


    }
}
