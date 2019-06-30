using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Private API - XML building

        [NotNull]
        static IEnumerable<XElement> GetCustomAttributeArgumentsXml([NotNull] ICustomAttribute customAttribute)
            => customAttribute.ConstructorArguments.Select(CustomAttributeArgumentToXml);

        [NotNull]
        static IEnumerable<XElement> GetAnnotationsXml([NotNull] ICustomAttributeProvider member)
            => member.CustomAttributes.Where(IsExportableJetBrainsAnnotation).Select(CustomAttributeToXml);

        [NotNull]
        static IEnumerable<XElement> GetGenericParametersXml([NotNull] IGenericParameterProvider provider)
            => provider.GenericParameters.Where(p => p.HasCustomAttributes).Select(GenericParameterToXml);

        [NotNull]
        static IEnumerable<XElement> GetParametersXml([NotNull] IEnumerable<ParameterDefinition> parameters)
            => parameters.Where(p => p.HasCustomAttributes).Select(ParameterToXml);

        [NotNull]
        static IEnumerable<XElement> GetMethodsXml([NotNull] TypeDefinition type)
            => type.Methods.Select(MethodToXml);

        [NotNull]
        static IEnumerable<XElement> GetPropertiesXml([NotNull] TypeDefinition type)
            => type.Properties.Select(PropertyToXml);

        [NotNull]
        static IEnumerable<XElement> GetFieldsXml([NotNull] TypeDefinition type)
            => type.Fields.Select(FieldToXml);

        [NotNull]
        static IEnumerable<XElement> GetEventsXml([NotNull] TypeDefinition type)
            => type.Events.Select(EventToXml);

        [NotNull]
        static IEnumerable<XElement> GetTypesXml([NotNull] AssemblyDefinition assembly)
            => assembly.Modules.SelectMany(m => m.GetTypes()).Where(IsExportedType).SelectMany(TypeToXml);

        #endregion
    }
}