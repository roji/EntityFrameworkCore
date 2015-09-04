﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;

namespace Microsoft.Data.Entity.Query.ExpressionVisitors
{
    public class CompositePredicateExpressionVisitorFactory : ICompositePredicateExpressionVisitorFactory
    {
        public virtual ExpressionVisitor Create(bool useRelationalNulls)
            => new CompositePredicateExpressionVisitor(useRelationalNulls);
    }
}
