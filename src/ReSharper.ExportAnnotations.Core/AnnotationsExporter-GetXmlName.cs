using System.Text;
using JetBrains.Annotations;
using Cecil.XmlDocNames;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Private API - GetXmlName overloads

        [NotNull]
        static string GetXmlName([NotNull] AssemblyDefinition assembly) => assembly.Name.Name;

        [NotNull]
        static string GetXmlName([NotNull] ParameterReference parameter) => parameter.Name;

        [NotNull]
        static string GetXmlName([NotNull] MemberReference member)
            => new StringBuilder().AppendXmlDocName(member).ToString();

        #endregion
    }
}