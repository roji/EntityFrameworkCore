// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Update;

#nullable enable

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public class NullableStructCurrentProviderValueComparer<TModel, TProvider> : IComparer<IUpdateEntry>
        where TModel : struct
    {
        private readonly IPropertyBase _property;
        private readonly IComparer<TProvider> _underlyingComparer;
        private readonly Func<TModel, TProvider> _converter;

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public NullableStructCurrentProviderValueComparer(
            [NotNull] IPropertyBase property,
            [NotNull] ValueConverter<TModel, TProvider> converter)
        {
            _property = property;
            _converter = converter.ConvertToProviderExpression.Compile();
            _underlyingComparer = Comparer<TProvider>.Default;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual int Compare(IUpdateEntry? x, IUpdateEntry? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            var xValue = x.GetCurrentValue<TModel?>(_property);
            var yValue = y.GetCurrentValue<TModel?>(_property);

            return !xValue.HasValue
                && !yValue.HasValue
                    ? 0
                    : !xValue.HasValue
                        ? -1
                        : !yValue.HasValue
                            ? 1
                            : _underlyingComparer.Compare(_converter(xValue.Value), _converter(yValue.Value));
        }
    }
}
