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
using JetBrains.Annotations;

namespace ReSharper.ExportAnnotations;

/// <summary>
/// Provides extension methods for <see cref="AnnotationsExporterParameters"/>.
/// </summary>
public static class AnnotationsExporterParametersExtensions
{
    /// <summary>
    /// Sets a value indicating whether to export annotations to a XML file.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="value"><c>true</c> to export annotations, <c>false</c> otherwise.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// property set to <paramref name="value"/>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// <seealso cref="WithExportAnnotations(AnnotationsExporterParameters)"/>
    /// <seealso cref="WithoutExportAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithExportAnnotations(this AnnotationsExporterParameters @this, bool value)
    {
        @this.ExportAnnotations = value;
        return @this;
    }

    /// <summary>
    /// Indicates to export annotations to a XML file.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// property set to <c>true</c>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// <seealso cref="WithExportAnnotations(AnnotationsExporterParameters,bool)"/>
    /// <seealso cref="WithoutExportAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithExportAnnotations(this AnnotationsExporterParameters @this) => WithExportAnnotations(@this, true);

    /// <summary>
    /// Indicates to not export annotations.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// property set to <c>false</c>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.ExportAnnotations"/>
    /// <seealso cref="WithExportAnnotations(AnnotationsExporterParameters,bool)"/>
    /// <seealso cref="WithExportAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithoutExportAnnotations(this AnnotationsExporterParameters @this) => WithExportAnnotations(@this, false);

    /// <summary>
    /// Sets a value indicating whether to strip annotations after (or instead of) exporting them.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="value"><c>true</c> to strip annotations, <c>false</c> otherwise.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// property set to <paramref name="value"/>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// <seealso cref="WithStripAnnotations(AnnotationsExporterParameters)"/>
    /// <seealso cref="WithoutStripAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithStripAnnotations(this AnnotationsExporterParameters @this, bool value)
    {
        @this.StripAnnotations = value;
        return @this;
    }

    /// <summary>
    /// Indicates to strip annotations after (or instead of) exporting them.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// property set to <c>true</c>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// <seealso cref="WithStripAnnotations(AnnotationsExporterParameters,bool)"/>
    /// <seealso cref="WithoutStripAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithStripAnnotations(this AnnotationsExporterParameters @this) => WithStripAnnotations(@this, true);

    /// <summary>
    /// Indicates to not strip annotations.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// property set to <c>false</c>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.StripAnnotations"/>
    /// <seealso cref="WithStripAnnotations(AnnotationsExporterParameters,bool)"/>
    /// <seealso cref="WithStripAnnotations(AnnotationsExporterParameters)"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithoutStripAnnotations(this AnnotationsExporterParameters @this) => WithStripAnnotations(@this, false);

    /// <summary>
    /// Sets the full path of the XML file where external annotations should be saved.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="path">The path of the XML file to create.</param>
    /// <returns><paramref name="this"/> with the <see cref="AnnotationsExporterParameters.XmlPath"/>
    /// property set to <paramref name="path"/>.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.XmlPath"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithXmlPath(this AnnotationsExporterParameters @this, string? path)
    {
        @this.XmlPath = path;
        return @this;
    }

    /// <summary>
    /// Adds a referenced library.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="path">The path to the library.</param>
    /// <returns><paramref name="this"/> with <paramref name="path"/> added to the <see cref="AnnotationsExporterParameters.Libraries"/>
    /// property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.Libraries"/>
    /// <seealso cref="AnnotationsExporterParameters.AddLibrary"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithLibrary(this AnnotationsExporterParameters @this, string path)
    {
        @this.AddLibrary(path);
        return @this;
    }

    /// <summary>
    /// Adds zero or more referenced libraries.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="paths">An enumeration of paths to libraries.</param>
    /// <returns><paramref name="this"/> with the elements of <paramref name="paths"/>
    /// added to the <see cref="AnnotationsExporterParameters.Libraries"/> property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="paths"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.Libraries"/>
    /// <seealso cref="AnnotationsExporterParameters.AddLibraries(IEnumerable{string})"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithLibraries(
        this AnnotationsExporterParameters @this,
        [InstantHandle] IEnumerable<string> paths)
    {
        @this.AddLibraries(paths);
        return @this;
    }

    /// <summary>
    /// Adds zero or more referenced libraries.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="paths">An array of paths to libraries.</param>
    /// <returns><paramref name="this"/> with the elements of <paramref name="paths"/>
    /// added to the <see cref="AnnotationsExporterParameters.Libraries"/> property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="paths"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.Libraries"/>
    /// <seealso cref="AnnotationsExporterParameters.AddLibraries(string[])"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithLibraries(this AnnotationsExporterParameters @this, params string[] paths)
    {
        @this.AddLibraries(paths);
        return @this;
    }

    /// <summary>
    /// Adds a directory where to search for referenced libraries.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="directory">The directory.</param>
    /// <returns><paramref name="this"/> with <paramref name="directory"/> added
    /// to the <see cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/> property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/>
    /// <seealso cref="AnnotationsExporterParameters.AddSearchDirectory"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithSearchDirectory(this AnnotationsExporterParameters @this, string directory)
    {
        @this.AddSearchDirectory(directory);
        return @this;
    }

    /// <summary>
    /// Adds zero or more directories where to search for referenced libraries.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="directories">An enumeration of paths to directories.</param>
    /// <returns><paramref name="this"/> with the elements of <paramref name="directories"/> added
    /// to the <see cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/> property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/>
    /// <seealso cref="AnnotationsExporterParameters.AddSearchDirectories(IEnumerable{string})"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithSearchDirectories(
        this AnnotationsExporterParameters @this,
        [InstantHandle] IEnumerable<string> directories)
    {
        @this.AddSearchDirectories(directories);
        return @this;
    }

    /// <summary>
    /// Adds zero or more directories where to search for referenced libraries.
    /// </summary>
    /// <param name="this">The <see cref="AnnotationsExporterParameters"/> on which this method is called.</param>
    /// <param name="directories">An array of paths to directories.</param>
    /// <returns><paramref name="this"/> with the elements of <paramref name="directories"/> added
    /// to the <see cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/> property.</returns>
    /// <exception cref="NullReferenceException"><paramref name="this"/> is <c>null</c>.</exception>
    /// <seealso cref="AnnotationsExporterParameters.AdditionalSearchDirectories"/>
    /// <seealso cref="AnnotationsExporterParameters.AddSearchDirectories(string[])"/>
    [PublicAPI]
    public static AnnotationsExporterParameters WithSearchDirectories(
        this AnnotationsExporterParameters @this,
        params string[] directories)
    {
        @this.AddSearchDirectories(directories);
        return @this;
    }
}
