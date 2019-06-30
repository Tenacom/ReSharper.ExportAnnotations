using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Private data

        static readonly IReadOnlyList<string> _nonExportableAttributeNames = new[] {
            "AspMvcSuppressViewErrorAttribute",
            "LocalizationRequiredAttribute",
            "MeansImplicitUseAttribute",
            "NoReorderAttribute",
            "PublicAPIAttribute",
            "UsedImplicitlyAttribute"
        };

        #endregion

        #region Private API - LINQ filters

        static bool IsExportableJetBrainsAnnotation([NotNull] CustomAttribute attribute)
            => attribute.AttributeType.Namespace == "JetBrains.Annotations"
            && !_nonExportableAttributeNames.Contains(attribute.AttributeType.Name, StringComparer.Ordinal);

        static bool IsExportedType([NotNull] TypeDefinition type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.Attributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                case TypeAttributes.NestedFamily:
                case TypeAttributes.NestedFamORAssem:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}