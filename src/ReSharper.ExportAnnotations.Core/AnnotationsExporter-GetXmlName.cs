// -----------------------------------------------------------------------------------
// Copyright (C) Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Text;
using Cecil.XmlDocNames;
using Mono.Cecil;

namespace ReSharper.ExportAnnotations
{
    /// <content />
    public partial class AnnotationsExporter
    {
        private static string GetXmlName(AssemblyDefinition assembly) => assembly.Name.Name;

        private static string GetXmlName(ParameterReference parameter) => parameter.Name;

        private static string GetXmlName(MemberReference member)
            => new StringBuilder().AppendXmlDocName(member).ToString();
    }
}
