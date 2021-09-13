// -----------------------------------------------------------------------------------
// Copyright (C) Riccardo De Agostini and Tenacom. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace ReSharper.ExportAnnotations.Internal
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
            where T : class
            => @this.Where(x => x != null)!;

        public static IEnumerable<T> PrependIfNotNull<T>(this IEnumerable<T> @this, T? item)
            where T : class
            => item == null ? @this : @this.Prepend(item);

#if NETFRAMEWORK
        // Emulate System.Linq.Enumerable.Prepend, introduced in .NET Framework 4.7.1 / .NET Standard 1.6
        // https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.prepend
        private static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T? item)
            where T : class
        {
            yield return item!;
            foreach (var originalItem in @this)
            {
                yield return originalItem;
            }
        }
#endif
    }
}
