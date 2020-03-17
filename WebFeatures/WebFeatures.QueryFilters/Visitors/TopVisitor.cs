﻿using System.Linq.Expressions;
using WebFeatures.QueryFilters.AntlrGenerated;
using WebFeatures.QueryFilters.Helpers;

namespace WebFeatures.QueryFilters.Visitors
{
    internal class TopVisitor : QueryFiltersBaseVisitor<object>
    {
        private readonly object _sourceQueryable;
        private readonly ParameterExpression _parameter;

        public TopVisitor(object sourceQueryable, ParameterExpression parameter)
        {
            _sourceQueryable = sourceQueryable;
            _parameter = parameter;
        }

        public override object VisitTop(QueryFiltersParser.TopContext context)
        {
            var take = ReflectionCache.Take
                .MakeGenericMethod(_parameter.Type);

            var count = int.Parse(context.count.Text);

            return take.Invoke(null, new[] { _sourceQueryable, count });
        }
    }
}
