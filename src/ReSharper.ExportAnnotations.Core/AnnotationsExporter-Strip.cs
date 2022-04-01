// ---------------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Tenacom. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations;

/// <content />
public partial class AnnotationsExporter
{
    private static void StripFrom(ICustomAttributeProvider provider)
    {
        if (!provider.HasCustomAttributes)
        {
            return;
        }

        var attributes = provider.CustomAttributes;
        var attributesToStrip = attributes.Where(a => a.AttributeType.Namespace == "JetBrains.Annotations").ToArray();
        foreach (var attribute in attributesToStrip)
        {
            _ = attributes.Remove(attribute);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void StripFromAllIf(bool condition, IEnumerable<ICustomAttributeProvider> providers)
    {
        if (!condition)
        {
            return;
        }

        foreach (var provider in providers)
        {
            StripFrom(provider);
        }
    }

    [PublicAPI]
    private static void StripAnnotations(AssemblyDefinition assembly)
    {
        StripFrom(assembly);
        foreach (var module in assembly.Modules.Where(m => m.HasAssemblyReferences))
        {
            var references = module.AssemblyReferences;
            var jetBrainsAnnotationsReference = references.FirstOrDefault(r =>
                r.Name.StartsWith("JetBrains.Annotations", StringComparison.Ordinal));
            if (jetBrainsAnnotationsReference == null)
            {
                continue;
            }

            StripFrom(module);
            foreach (var type in module.GetTypes())
            {
                StripFrom(type);
                StripFromAllIf(type.HasGenericParameters, type.GenericParameters);
                StripFromAllIf(type.HasFields, type.Fields);
                StripFromAllIf(type.HasEvents, type.Events);
                StripFromAllIf(type.HasProperties, type.Properties);
                if (!type.HasMethods)
                {
                    continue;
                }

                foreach (var method in type.Methods)
                {
                    StripFrom(method);
                    StripFrom(method.MethodReturnType);
                    StripFromAllIf(method.HasGenericParameters, method.GenericParameters);
                    StripFromAllIf(method.HasParameters, method.Parameters);
                }
            }

            _ = references.Remove(jetBrainsAnnotationsReference);
        }
    }
}
