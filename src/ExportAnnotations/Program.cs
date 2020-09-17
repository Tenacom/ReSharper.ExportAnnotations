// -----------------------------------------------------------------------------------
// Copyright (C) Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;
using ReSharper.ExportAnnotations;

namespace ExportAnnotations
{
    public sealed class Program
    {
        [Argument(0, "assemblyPath", Description = "Full path of the compiled assembly")]
        [Required]
        [FileExists]
        [UsedImplicitly]
        public string AssemblyPath { get; } = string.Empty;

        [Option(
            Description = "Do not export annotations (but strip them if --strip is specified)",
            LongName = "no-export",
            ShortName = "n")]
        [UsedImplicitly]
        public bool NoExport { get; }

        [Argument(1, "xmlPath", Description = "Full path of the generated XML file (defaults to a .ExternalAnnotations.xml file side by side with the assembly)")]
        [LegalFilePath]
        [UsedImplicitly]
        public string? XmlPath { get; }

        [Option(
            CommandOptionType.MultipleValue,
            Description = "Full path of a referenced library (will be actually referenced only when rewriting the assembly)",
            LongName = "lib",
            ShortName = "l")]
        [LegalFilePath]
        [UsedImplicitly]
        public IEnumerable<string>? LibraryPaths { get; }

        [Option(
            Description = "Full path of a text file containing a referenced library path on each line",
            LongName = "liblist",
            ShortName = "ll")]
        [LegalFilePath]
        [UsedImplicitly]
        public string? LibraryListPath { get; }

        [Option(Description = "Strip annotations from assembly (CAUTION: will also strip debug symbols on non-Windows systems)")]
        [UsedImplicitly]
        public bool Strip { get; }

        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [UsedImplicitly]
        private int OnExecute()
        {
            var listedLibraries = LibraryListPath != null
                ? File.ReadAllLines(LibraryListPath).Where(p => !string.IsNullOrWhiteSpace(p)).Where(File.Exists)
                : Array.Empty<string>();

            var parameters = new AnnotationsExporterParameters()
                .WithExportAnnotations(!NoExport)
                .WithXmlPath(XmlPath)
                .WithLibraries((LibraryPaths ?? Enumerable.Empty<string>()).Concat(listedLibraries))
                .WithStripAnnotations(Strip);

            AnnotationsExporter.Run(AssemblyPath, parameters);

            return 0;
        }
    }
}