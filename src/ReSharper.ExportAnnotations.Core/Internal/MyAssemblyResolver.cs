using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations.Internal
{
    sealed class MyAssemblyResolver : DefaultAssemblyResolver
    {
        #region Private data

        static readonly IEnumerable<string> WindowsRuntimeExtensions = new[] { ".winmd", ".dll" };
        static readonly IEnumerable<string> NonWindowsRuntimeExtensions = new[] { ".exe", ".dll" };

readonly IReadOnlyList<string> _referencedLibPaths;

        #endregion

        #region Instance management

        public MyAssemblyResolver([NotNull] string dllPath, [NotNull, ItemNotNull] IEnumerable<string> referencedLibPaths)
        {
            AddSearchDirectory(Path.GetDirectoryName(dllPath));

            _referencedLibPaths = referencedLibPaths.ToArray();
        }

        #endregion

        #region Private API

        AssemblyDefinition GetAssembly([NotNull] string file, [NotNull] ReaderParameters parameters)
        {
            parameters.AssemblyResolver ??= this;

            return ModuleDefinition.ReadModule(file, parameters).Assembly;
        }

        #endregion

        #region BaseAssemblyResolver overrides

        public override AssemblyDefinition Resolve([NotNull] AssemblyNameReference name, [NotNull] ReaderParameters parameters)
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

        #endregion
    }
}