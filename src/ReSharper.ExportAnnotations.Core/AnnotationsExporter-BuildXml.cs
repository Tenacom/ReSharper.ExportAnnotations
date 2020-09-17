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
        private static IEnumerable<XElement> GetCustomAttributeArgumentsXml(ICustomAttribute customAttribute)
            => customAttribute.ConstructorArguments.Select(CustomAttributeArgumentToXml);

        private static IEnumerable<XElement> GetAnnotationsXml(ICustomAttributeProvider member)
            => member.CustomAttributes.Where(IsExportableJetBrainsAnnotation).Select(CustomAttributeToXml);

        private static IEnumerable<XElement> GetGenericParametersXml(IGenericParameterProvider provider)
            => provider.GenericParameters
                .Where(p => p.HasCustomAttributes)
                .Select(GenericParameterToXml)
                .WhereNotNull();

        private static IEnumerable<XElement> GetParametersXml(IEnumerable<ParameterDefinition> parameters)
            => parameters.Where(p => p.HasCustomAttributes).Select(ParameterToXml).WhereNotNull();

        private static IEnumerable<XElement> GetMethodsXml(TypeDefinition type)
            => type.Methods.Select(MethodToXml).WhereNotNull();

        private static IEnumerable<XElement> GetPropertiesXml(TypeDefinition type)
            => type.Properties.Select(PropertyToXml).WhereNotNull();

        private static IEnumerable<XElement> GetFieldsXml(TypeDefinition type)
            => type.Fields.Select(FieldToXml).WhereNotNull();

        private static IEnumerable<XElement> GetEventsXml(TypeDefinition type)
            => type.Events.Select(EventToXml).WhereNotNull();

        private static IEnumerable<XElement> GetTypesXml(AssemblyDefinition assembly)
            => assembly.Modules.SelectMany(m => m.GetTypes()).Where(IsExportedType).SelectMany(TypeToXml);
    }
}