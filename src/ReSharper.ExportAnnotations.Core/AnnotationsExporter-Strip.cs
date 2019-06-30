using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    partial class AnnotationsExporter
    {
        #region Private API

        static void StripFrom([NotNull] ICustomAttributeProvider provider)
        {
            if (!provider.HasCustomAttributes)
                return;

            var attributes = provider.CustomAttributes;
            var attributesToStrip = attributes.Where(a => a.AttributeType.Namespace == "JetBrains.Annotations").ToArray();
            foreach (var attribute in attributesToStrip)
                attributes.Remove(attribute);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void StripFromAllIf(bool condition, [NotNull, ItemNotNull] IEnumerable<ICustomAttributeProvider> providers)
        {
            if (!condition)
                return;

            foreach (var provider in providers)
                StripFrom(provider);
        }

        [PublicAPI]
        static void StripAnnotations([NotNull] AssemblyDefinition assembly)
        {
            StripFrom(assembly);
            foreach (var module in assembly.Modules.Where(m => m.HasAssemblyReferences))
            {
                var references = module.AssemblyReferences;
                var jetBrainsAnnotationsReference = references.FirstOrDefault(r =>
                    r.Name.StartsWith("JetBrains.Annotations", StringComparison.Ordinal));
                if (jetBrainsAnnotationsReference == null)
                    continue;

                StripFrom(module);
                foreach (var type in module.GetTypes())
                {
                    StripFrom(type);
                    StripFromAllIf(type.HasGenericParameters, type.GenericParameters);
                    StripFromAllIf(type.HasFields, type.Fields);
                    StripFromAllIf(type.HasEvents, type.Events);
                    StripFromAllIf(type.HasProperties, type.Properties);
                    if (!type.HasMethods)
                        continue;

                    foreach (var method in type.Methods)
                    {
                        StripFrom(method);
                        StripFrom(method.MethodReturnType);
                        StripFromAllIf(method.HasGenericParameters, method.GenericParameters);
                        StripFromAllIf(method.HasParameters, method.Parameters);
                    }
                }

                references.Remove(jetBrainsAnnotationsReference);
            }
        }

        #endregion
    }
}