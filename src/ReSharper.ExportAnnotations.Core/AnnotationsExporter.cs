using System;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Mono.Cecil;
using ReSharper.ExportAnnotations.Internal;

namespace ReSharper.ExportAnnotations
{
    /// <summary>
    /// Exports and/or strips ReSharper annotations from compiled assemblies.
    /// </summary>
    /// <seealso cref="Run(string,AnnotationsExporterParameters)"/>
    /// <seealso cref="Run(string,Action{AnnotationsExporterParameters})"/>
    [PublicAPI]
    public static partial class AnnotationsExporter
    {
        #region Public API

        /// <summary>
        /// Exports and/or strips ReSharper annotations from a compiled assembly,
        /// according to a <see cref="AnnotationsExporterParameters"/> object
        /// configured by a given callback.
        /// </summary>
        /// <param name="assemblyPath">The path to the compiled assembly.</param>
        /// <param name="configure">A callback that is called with a newly-constructed
        /// <see cref="AnnotationsExporterParameters"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="assemblyPath"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="configure"/> is <c>null</c>.</para>
        /// </exception>
        /// <seealso cref="Run(string,AnnotationsExporterParameters)"/>
        /// <seealso cref="AnnotationsExporterParameters"/>
        [PublicAPI]
        public static void Run([NotNull] string assemblyPath, [NotNull] Action<AnnotationsExporterParameters> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var parameters = new AnnotationsExporterParameters();
            configure(parameters);
            Run(assemblyPath, parameters);
        }

        /// <summary>
        /// Exports and/or strips ReSharper annotations from a compiled assembly,
        /// according to a given <see cref="AnnotationsExporterParameters"/> object.
        /// </summary>
        /// <param name="assemblyPath">The path to the compiled assembly.</param>
        /// <param name="parameters">An <see cref="AnnotationsExporterParameters"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assemblyPath"/> is <c>null</c>.</exception>
        /// <remarks>
        /// <para>If <paramref name="parameters"/> is <c>null</c>, an <see cref="AnnotationsExporterParameters"/>
        /// object will be automatically created by calling the default constructor.</para>
        /// </remarks>
        /// <seealso cref="Run(string,Action{AnnotationsExporterParameters})"/>
        /// <seealso cref="AnnotationsExporterParameters"/>
        /// <seealso cref="C:AnnotationsExporterParameters()"/>
        [PublicAPI]
        public static void Run([NotNull] string assemblyPath, [CanBeNull] AnnotationsExporterParameters parameters)
        {
            if (assemblyPath == null)
                throw new ArgumentNullException(nameof(assemblyPath));

            parameters ??= new AnnotationsExporterParameters();

            var assemblyResolver = new MyAssemblyResolver(assemblyPath, parameters.Libraries);
            foreach (var directory in parameters.AdditionalSearchDirectories)
                assemblyResolver.AddSearchDirectory(directory);

            var readerParameters = new ReaderParameters
            {
                ReadingMode = ReadingMode.Deferred,
                ReadWrite = parameters.WillSaveAssembly,
                AssemblyResolver = assemblyResolver,
                ReadSymbols = true
            };

            using (var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters))
            {
                if (parameters.ExportAnnotations)
                    ExportAnnotations(assembly,
                        parameters.XmlPath ?? Path.ChangeExtension(assemblyPath, ".ExternalAnnotations.xml"));

                if (parameters.StripAnnotations)
                    StripAnnotations(assembly);

                if (parameters.WillSaveAssembly)
                {
                    var writerParameters = new WriterParameters {
                        WriteSymbols = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) // Symbol writer uses COM Interop, thus is Windows-only
                    };
                    assembly.Write(writerParameters);
                }
            }
        }

        #endregion
    }
}