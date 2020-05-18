﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebFeatures.Application.Interfaces.DataAccess.Repositories;
using WebFeatures.Domian.Common;

namespace WebFeatures.Infrastructure.DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected IDbConnection Connection => _transaction.Connection;
        private readonly IDbTransaction _transaction;

        protected BaseRepository(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public abstract Task CreateAsync(TEntity entity);
        public abstract Task DeleteAsync(TEntity entity);
        public abstract Task<bool> ExistsAsync(Guid id);
        public abstract Task<IEnumerable<TEntity>> GetAllAsync();
        public abstract Task<TEntity> GetAsync(Guid id);
        public abstract Task UpdateAsync(TEntity entity);
    }
}
