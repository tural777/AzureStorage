using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : TableEntity, new()
    {
        // Azure table-lerde ishler gormek uchun
        private readonly CloudTableClient _cloudTableClient;

        // Azurda sadece 1 table ile ishlemek uchun
        private readonly CloudTable _table;


        public TableStorage()
        {
            // Azure accountumuza baglanmaq
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionStrings.AzureStorageConnectionString);

            _cloudTableClient = storageAccount.CreateCloudTableClient();

            _table = _cloudTableClient.GetTableReference(typeof(TEntity).Name);

            _table.CreateIfNotExists();
        }


        public async Task<TEntity> Add(TEntity entity)
        {
            var operation = TableOperation.Insert(entity);

            var execute = await _table.ExecuteAsync(operation);

            // Result ugurla basha chatibsa obj geri qaytarir.
            // Eks halda return null olacaq
            return execute.Result as TEntity;
        }


        public IQueryable<TEntity> All()
        {
            return _table.CreateQuery<TEntity>().AsQueryable();
        }


        public async Task Delete(string rowKey, string partitionKey)
        {
            var entity = await Get(rowKey, partitionKey);

            var operation = TableOperation.Delete(entity);

            await _table.ExecuteAsync(operation);
        }


        public async Task<TEntity> Get(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);

            var execute = await _table.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }


        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return _table.CreateQuery<TEntity>().Where(query);
        }


        public async Task<TEntity> Update(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);

            var execute = await _table.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }
    }
}
