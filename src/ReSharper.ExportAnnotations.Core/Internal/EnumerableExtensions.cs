// -----------------------------------------------------------------------------------
// Copyright (C) Tenacom. All rights reserved.
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
    }
}