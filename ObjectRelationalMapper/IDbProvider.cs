using System.Collections.Generic;
using System.Data.SqlClient;
using LogicalExpressionGenerator;

namespace ObjectRelationalMapper
{
    public interface IDbProvider
    {
        #region Properties

        string ConnectionString { get; set; }

        #endregion

        #region  Public Methods

        void Insert(string tableName, params DataProperty[] propertyValues);

        List<DataProperty[]> Select(string tableName, Condition condition, string[] fields);

        void Delete(string tableName, Condition condition);

        void Update(string tableName, Condition condition, params DataProperty[] propertyValues);

        SqlTransaction BeginTransaction();

        void CommitTransaction(SqlTransaction transaction);

        void RollbackTransaction(SqlTransaction transaction);

        #endregion
    }
}