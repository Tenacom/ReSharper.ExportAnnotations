// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    /// <content />
    public partial class AnnotationsExporter
    {
        private static readonly IReadOnlyList<string> NonExportableAttributeNames = new[]
        {
            "AspMvcSuppressViewErrorAttribute",
            "LocalizationRequiredAttribute",
            "MeansImplicitUseAttribute",
            "NoReorderAttribute",
            "PublicAPIAttribute",
            "UsedImplicitlyAttribute",
        };

        private static bool IsExportableJetBrainsAnnotation(CustomAttribute attribute)
            => attribute.AttributeType.Namespace == "JetBrains.Annotations"
            && !NonExportableAttributeNames.Contains(attribute.AttributeType.Name, StringComparer.Ordinal);

        private static bool IsExportedType(TypeDefinition type)
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
    }
}
