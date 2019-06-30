using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using Mono.Cecil;
using ReSharper.ExportAnnotations.Internal;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Private API - LINQ selectors

        [CanBeNull]
        static XElement CustomAttributeArgumentToXml(CustomAttributeArgument argument)
            => new XElement("argument", argument.Value.ToString());

        [NotNull]
        static XElement CustomAttributeToXml([NotNull] CustomAttribute customAttribute)
            => new XElementBuilder("attribute", "ctor", GetXmlName(customAttribute.Constructor))
                .AddRange(GetCustomAttributeArgumentsXml(customAttribute))
                .EnsureElement();

        [CanBeNull]
        static XElement GenericParameterToXml([NotNull] GenericParameter parameter)
            => new XElementBuilder("typeparameter", "name", GetXmlName(parameter))
                .AddRange(GetAnnotationsXml(parameter))
                .Element;

        [CanBeNull]
        static XElement ParameterToXml([NotNull] ParameterDefinition parameter)
            => new XElementBuilder("parameter", "name", GetXmlName(parameter))
                .AddRange(GetAnnotationsXml(parameter))
                .Element;

        [CanBeNull]
        static XElement MethodToXml([NotNull] MethodDefinition method)
            => new XElementBuilder("member", "name", GetXmlName(method))
                .AddRange(GetAnnotationsXml(method))
                .AddRange(GetGenericParametersXml(method))
                .AddRange(GetParametersXml(method.Parameters))
                .Element;

        [CanBeNull]
        static XElement PropertyToXml([NotNull] PropertyDefinition property)
            => new XElementBuilder("member", "name", GetXmlName(property))
                .AddRange(GetAnnotationsXml(property))
                .AddRange(GetParametersXml(property.Parameters))
                .Element;

        [CanBeNull]
        static XElement FieldToXml([NotNull] FieldDefinition field)
            => new XElementBuilder("member", "name", GetXmlName(field))
                .AddRange(GetAnnotationsXml(field))
                .Element;

        [CanBeNull]
        static XElement EventToXml([NotNull] EventDefinition @event)
            => new XElementBuilder("member", "name", GetXmlName(@event))
                .AddRange(GetAnnotationsXml(@event))
                .Element;

        [CanBeNull]
        static IEnumerable<XElement> TypeToXml([NotNull] TypeDefinition type)
            => GetMethodsXml(type)
                .Concat(GetPropertiesXml(type))
                .Concat(GetFieldsXml(type))
                .Concat(GetEventsXml(type))
                .Prepend(new XElementBuilder("member", "name", GetXmlName(type))
                    .AddRange(GetAnnotationsXml(type))
                    .AddRange(GetGenericParametersXml(type))
                    .Element);

        [NotNull]
        static XElement AssemblyToXml([NotNull] AssemblyDefinition assembly)
            => new XElementBuilder("assembly", "name", GetXmlName(assembly))
                .AddRange(GetTypesXml(assembly))
                .EnsureElement();

        #endregion
    }
}