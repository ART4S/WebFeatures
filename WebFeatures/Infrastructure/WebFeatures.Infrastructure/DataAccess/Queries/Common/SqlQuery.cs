﻿using WebFeatures.Common;

namespace WebFeatures.Infrastructure.DataAccess.Queries.Common
{
    internal class SqlQuery
    {
        public string Query { get; }
        public object Param { get; }
        public string SplitOn { get; }

        public SqlQuery(string query, object param = null, string splitOn = "Id")
        {
            Guard.ThrowIfNull(query, nameof(query));
            Guard.ThrowIfNull(splitOn, nameof(splitOn));

            Query = query;
            Param = param;
            SplitOn = splitOn;
        }
    }
}
