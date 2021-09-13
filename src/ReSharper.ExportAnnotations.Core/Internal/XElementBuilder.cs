// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Linq;

namespace ReSharper.ExportAnnotations.Internal
{
    internal sealed class XElementBuilder
    {
        internal XElementBuilder(string tag, string nameAttribute, string name)
        {
            Tag = tag;
            NameAttribute = nameAttribute;
            Name = name;
        }

        public XElement? Element { get; private set; }

        private string Tag { get; }

        private string NameAttribute { get; }

        private string Name { get; }

        public XElement EnsureElement() => Element ??= new XElement(Tag, new XAttribute(NameAttribute, Name));

        public XElementBuilder AddRange(IEnumerable<XElement?> children)
        {
            foreach (var child in children.WhereNotNull())
            {
                EnsureElement().Add(child);
            }

            return this;
        }
    }
}
