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
using JetBrains.Annotations;

namespace ReSharper.ExportAnnotations
{
    /// <summary>
    /// Holds parameters for <see cref="AnnotationsExporter"/>.
    /// </summary>
    public sealed class AnnotationsExporterParameters
    {
        private readonly List<string> _libraries = new List<string>();
        private readonly List<string> _additionalSearchDirectories = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationsExporterParameters"/> class,
        /// with the <see cref="ExportAnnotations"/> property set to <see langword="true"/>
        /// and all other properties set to their default values.
        /// </summary>
        [PublicAPI]
        public AnnotationsExporterParameters()
        {
            ExportAnnotations = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to export annotations
        /// to a XML file.
        /// </summary>
        [PublicAPI]
        public bool ExportAnnotations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to strip annotations
        /// from an assembly after, or instead of, exporting them.
        /// </summary>
        [PublicAPI]
        public bool StripAnnotations { get; set; }

        /// <summary>
        /// <para>Gets a list of libraries referenced by the treated assembly.</para>
        /// <para>The libraries in this list will be actually referenced only if and when
        /// rewriting the assembly, i.e. if <see cref="StripAnnotations"/> is <c>true</c>.</para>
        /// </summary>
        /// <seealso cref="AddLibrary"/>
        /// <seealso cref="AddLibraries(IEnumerable{string})"/>
        /// <seealso cref="AddLibraries(string[])"/>
        [PublicAPI]
        public IReadOnlyList<string> Libraries => _libraries;

        /// <summary>
        /// Gets a list of directories where to search for referenced libraries.
        /// </summary>
        /// <seealso cref="AddSearchDirectory"/>
        /// <seealso cref="AddSearchDirectories(IEnumerable{string})"/>
        /// <seealso cref="AddSearchDirectories(string[])"/>
        [PublicAPI]
        public IReadOnlyList<string> AdditionalSearchDirectories => _additionalSearchDirectories;

        /// <summary>
        /// <para>Gets or sets the full path of the XML file where external annotations should be saved.</para>
        /// <para>If left to <c>null</c> (the default), will cause external annotations to be saved in a file
        /// in the same directory and with the same name as the assembly, having a
        /// <c>.ExternalAnnotations.xml</c> extension.</para>
        /// </summary>
        [PublicAPI]
        public string? XmlPath { get; set; }

        /// <summary>
        /// <para>Gets a value indicating whether the options currently set on this instance
        /// will trigger a rewrite of the assembly.</para>
        /// <para>Currently an assembly is rewritten only when the <see cref="StripAnnotations"/>
        /// property is <c>true</c>.</para>
        /// </summary>
        [PublicAPI]
        public bool WillSaveAssembly => StripAnnotations;

        /// <summary>
        /// Adds a referenced library.
        /// </summary>
        /// <param name="path">The path to the library.</param>
        /// <seealso cref="Libraries"/>
        /// <seealso cref="AddLibraries(IEnumerable{string})"/>
        /// <seealso cref="AddLibraries(string[])"/>
        [PublicAPI]
        public void AddLibrary(string path) => _libraries.Add(path);

        /// <summary>
        /// Adds zero or more referenced libraries.
        /// </summary>
        /// <param name="paths">An enumeration of paths to libraries.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paths"/> is <c>null</c>.</exception>
        /// <seealso cref="Libraries"/>
        /// <seealso cref="AddLibrary"/>
        /// <seealso cref="AddLibraries(string[])"/>
        [PublicAPI]
        public void AddLibraries([InstantHandle] IEnumerable<string> paths)
            => _libraries.AddRange(paths);

        /// <summary>
        /// Adds zero or more referenced libraries.
        /// </summary>
        /// <param name="paths">An array of paths to libraries.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paths"/> is <c>null</c>.</exception>
        /// <seealso cref="Libraries"/>
        /// <seealso cref="AddLibrary"/>
        /// <seealso cref="AddLibraries(IEnumerable{string})"/>
        [PublicAPI]
        public void AddLibraries([InstantHandle] params string[] paths)
            => _libraries.AddRange(paths);

        /// <summary>
        /// Adds a directory where to search for referenced libraries.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <seealso cref="AdditionalSearchDirectories"/>
        /// <seealso cref="AddSearchDirectories(IEnumerable{string})"/>
        /// <seealso cref="AddSearchDirectories(string[])"/>
        [PublicAPI]
        public void AddSearchDirectory(string directory)
            => _additionalSearchDirectories.Add(directory);

        /// <summary>
        /// Adds zero or more directories where to search for referenced libraries.
        /// </summary>
        /// <param name="directories">An enumeration of paths to directories.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directories"/> is <c>null</c>.</exception>
        /// <seealso cref="AdditionalSearchDirectories"/>
        /// <seealso cref="AddSearchDirectory"/>
        /// <seealso cref="AddSearchDirectories(string[])"/>
        [PublicAPI]
        public void AddSearchDirectories([InstantHandle] IEnumerable<string> directories)
            => _additionalSearchDirectories.AddRange(directories);

        /// <summary>
        /// Adds zero or more directories where to search for referenced libraries.
        /// </summary>
        /// <param name="directories">An array of paths to directories.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directories"/> is <c>null</c>.</exception>
        /// <seealso cref="AdditionalSearchDirectories"/>
        /// <seealso cref="AddSearchDirectory"/>
        /// <seealso cref="AddSearchDirectories(IEnumerable{string})"/>
        [PublicAPI]
        public void AddSearchDirectories([InstantHandle] params string[] directories)
            => _additionalSearchDirectories.AddRange(directories);
    }
}