// ---------------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Tenacom. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations.Internal;

internal sealed class MyAssemblyResolver : DefaultAssemblyResolver
{
    private static readonly IEnumerable<string> WindowsRuntimeExtensions = new[] { ".winmd", ".dll" };
    private static readonly IEnumerable<string> NonWindowsRuntimeExtensions = new[] { ".exe", ".dll" };

    private readonly IReadOnlyList<string> _referencedLibPaths;

    public MyAssemblyResolver(string dllPath, IEnumerable<string> referencedLibPaths)
    {
        AddSearchDirectory(Path.GetDirectoryName(dllPath));

        _referencedLibPaths = referencedLibPaths.ToArray();
    }

    public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
    {
        var extensions = name.IsWindowsRuntime ? WindowsRuntimeExtensions : NonWindowsRuntimeExtensions;
        var paths = _referencedLibPaths
            .Where(File.Exists)
            .Where(p => Path.GetFileNameWithoutExtension(p) == name.Name)
            .Where(p => extensions.Contains(Path.GetExtension(p)));

        foreach (var path in paths)
        {
            try
            {
                return GetAssembly(path, parameters);
            }
            catch (System.BadImageFormatException)
            {
            }
        }

        return base.Resolve(name, parameters);
    }

    private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
    {
        parameters.AssemblyResolver ??= this;

        return ModuleDefinition.ReadModule(file, parameters).Assembly;
    }
}
