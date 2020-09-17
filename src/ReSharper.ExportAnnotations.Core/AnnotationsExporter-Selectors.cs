// -----------------------------------------------------------------------------------
// Copyright (C) Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using ReSharper.ExportAnnotations.Internal;

namespace ReSharper.ExportAnnotations
{
    /// <content />
    public partial class AnnotationsExporter
    {
        private static XElement CustomAttributeArgumentToXml(CustomAttributeArgument argument)
            => new XElement("argument", argument.Value.ToString());

        private static XElement CustomAttributeToXml(CustomAttribute customAttribute)
            => new XElementBuilder("attribute", "ctor", GetXmlName(customAttribute.Constructor))
                .AddRange(GetCustomAttributeArgumentsXml(customAttribute))
                .EnsureElement();

        private static XElement? GenericParameterToXml(GenericParameter parameter)
            => new XElementBuilder("typeparameter", "name", GetXmlName(parameter))
                .AddRange(GetAnnotationsXml(parameter))
                .Element;

        private static XElement? ParameterToXml(ParameterDefinition parameter)
            => new XElementBuilder("parameter", "name", GetXmlName(parameter))
                .AddRange(GetAnnotationsXml(parameter))
                .Element;

        private static XElement? MethodToXml(MethodDefinition method)
            => new XElementBuilder("member", "name", GetXmlName(method))
                .AddRange(GetAnnotationsXml(method))
                .AddRange(GetGenericParametersXml(method))
                .AddRange(GetParametersXml(method.Parameters))
                .Element;

        private static XElement? PropertyToXml(PropertyDefinition property)
            => new XElementBuilder("member", "name", GetXmlName(property))
                .AddRange(GetAnnotationsXml(property))
                .AddRange(GetParametersXml(property.Parameters))
                .Element;

        private static XElement? FieldToXml(FieldDefinition field)
            => new XElementBuilder("member", "name", GetXmlName(field))
                .AddRange(GetAnnotationsXml(field))
                .Element;

        private static XElement? EventToXml(EventDefinition @event)
            => new XElementBuilder("member", "name", GetXmlName(@event))
                .AddRange(GetAnnotationsXml(@event))
                .Element;

        private static IEnumerable<XElement> TypeToXml(TypeDefinition type)
            => GetMethodsXml(type)
                .Concat(GetPropertiesXml(type))
                .Concat(GetFieldsXml(type))
                .Concat(GetEventsXml(type))
                .PrependIfNotNull(new XElementBuilder("member", "name", GetXmlName(type))
                    .AddRange(GetAnnotationsXml(type))
                    .AddRange(GetGenericParametersXml(type))
                    .Element);

        private static XElement AssemblyToXml(AssemblyDefinition assembly)
            => new XElementBuilder("assembly", "name", GetXmlName(assembly))
                .AddRange(GetTypesXml(assembly))
                .EnsureElement();
    }
}