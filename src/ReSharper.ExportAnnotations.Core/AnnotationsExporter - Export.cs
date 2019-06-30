using System.Xml.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Public API

        [PublicAPI]
        static void ExportAnnotations([NotNull] AssemblyDefinition assembly, [NotNull] string xmlPath)
        {
            var xml = new XDocument(AssemblyToXml(assembly));
            xml.Save(xmlPath, SaveOptions.None);
        }

        #endregion
    }
}