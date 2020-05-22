﻿using Dapper;
using Npgsql;
using WebFeatures.Infrastructure.Tests.Helpers;

namespace WebFeatures.Infrastructure.Tests.Fixtures
{
    public class NpgsqlDatabaseFixture : DatabaseFixture<NpgsqlConnection>
    {
        private const string _databaseName = "webfeatures_test_db";

        public NpgsqlDatabaseFixture() : base(new NpgsqlConnection("server=localhost;port=5432;username=postgres;password=postgres"))
        {
            Init();
        }

        private void Init()
        {
            CreateDatabase();
            CreateSchema();
            SeedTestData();
        }

        private void CreateDatabase()
        {
            Connection.Execute(SqlBuilder.CloseExistingConnections(_databaseName));
            Connection.Execute(SqlBuilder.DropDatabase(_databaseName));
            Connection.Execute(SqlBuilder.CreateDatabase(_databaseName));

            Connection.ChangeDatabase(_databaseName);
        }

        private void CreateSchema()
        {
            Connection.Execute(SqlBuilder.CreateSchema());
        }

        private void SeedTestData()
        {
            DataSeeder.SeedTestData(Connection);
        }

        public override void Dispose()
        {
            Connection.ChangeDatabase("postgres");

            Connection.Execute(SqlBuilder.CloseExistingConnections(_databaseName));
            Connection.Execute(SqlBuilder.DropDatabase(_databaseName));

            base.Dispose();
        }
    }
}
