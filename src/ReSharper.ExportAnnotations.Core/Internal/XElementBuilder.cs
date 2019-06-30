using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace ReSharper.ExportAnnotations.Internal
{
    sealed class XElementBuilder
    {
        #region Instance management

        internal XElementBuilder([NotNull] string tag, [NotNull] string nameAttribute, [NotNull] string name)
        {
            Tag = tag;
            NameAttribute = nameAttribute;
            Name = name;
        }

        #endregion

        #region Public API

        [CanBeNull]
        public XElement Element { get; private set; }

        [NotNull]
        public XElement EnsureElement() => Element ?? (Element = new XElement(Tag, new XAttribute(NameAttribute, Name)));

        [NotNull]
        public XElementBuilder AddRange([NotNull] IEnumerable<XElement> children)
        {
            foreach (var child in children.Where(c => c != null))
                EnsureElement().Add(child);

            return this;
        }

        #endregion

        #region Private API

        [NotNull]
        string Tag { get; }

        [NotNull]
        string NameAttribute { get; }

        [NotNull]
        string Name { get; }

        #endregion
    }
}