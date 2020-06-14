﻿using Npgsql;
using System.Data;
using WebFeatures.Common;

namespace WebFeatures.DbCreator.Core.DataAccess
{
    internal class PostgreSqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public PostgreSqlDbConnectionFactory(string connectionString)
        {
            Guard.ThrowIfNullOrEmpty(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}