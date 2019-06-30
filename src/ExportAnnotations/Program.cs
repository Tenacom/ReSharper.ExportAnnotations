using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;
using ReSharper.ExportAnnotations;

namespace ExportAnnotations
{
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "assemblyPath", Description = "Full path of the compiled assembly")]
        [Required]
        [FileExists]
        [UsedImplicitly]
        public string AssemblyPath { get; }

        [Argument(1, "xmlPath", Description = "Full path of the generated XML file (defaults to a .ExternalAnnotations.xml file side by side with the assembly)")]
        [LegalFilePath]
        [UsedImplicitly]
        public string XmlPath { get; }

        [Option(CommandOptionType.MultipleValue,
            Description = "Full path of a referenced library (will be actually referenced only when rewriting the assembly)",
            LongName = "lib",
            ShortName = "l")]
        [LegalFilePath]
        [UsedImplicitly]
        public string[] LibraryPaths { get; }

        [Option(Description = "Full path of a text file containing a referenced library path on each line",
            LongName = "liblist",
            ShortName = "ll")]
        [LegalFilePath]
        [UsedImplicitly]
        public string LibraryListPath { get; }

        [Option(Description = "Strip annotations from assembly (CAUTION: will also strip debug symbols if not on Windows)")]
        [UsedImplicitly]
        public bool Strip { get; }

        [UsedImplicitly]
        int OnExecute()
        {
            var listedLibraries = LibraryListPath != null
                ? File.ReadAllLines(LibraryListPath).Where(p => !string.IsNullOrWhiteSpace(p)).Where(File.Exists)
                : Array.Empty<string>();

            var parameters = new AnnotationsExporterParameters()
                .WithXmlPath(XmlPath)
                .WithLibraries((LibraryPaths ?? Enumerable.Empty<string>()).Concat(listedLibraries))
                .WithStripAnnotations(Strip);

            AnnotationsExporter.Run(AssemblyPath, parameters);
            
            return 0;
        }
    }
}