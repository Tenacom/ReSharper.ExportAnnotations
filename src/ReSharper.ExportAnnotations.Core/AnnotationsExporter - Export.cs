// -----------------------------------------------------------------------------------
// Copyright (C) Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Xml.Linq;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    /// <content />
    public partial class AnnotationsExporter
    {
        [PublicAPI]
        private static void ExportAnnotations(AssemblyDefinition assembly, string xmlPath)
        {
            var xml = new XDocument(AssemblyToXml(assembly));
            xml.Save(xmlPath, SaveOptions.None);
        }
    }
}