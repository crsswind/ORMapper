using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LogicalExpressionGenerator;

namespace ObjectRelationalMapper
{
    public class SqlDataProvider : IDbProvider
    {
        #region Fields

        private readonly List<TransactionInfo> _transactionInfo = new List<TransactionInfo>();

        #endregion

        #region IDbProvider Members

        public string ConnectionString { get; set; }

        public void Insert(string tableName, params DataProperty[] propertyValues)
        {
            string names = propertyValues.Select(n => $"{n.Name}").Aggregate((s1, s2) => s1 + "," + s2);
            string values = propertyValues.Select(n => $"'@{n.Value}'").Aggregate((s1, s2) => s1 + "," + s2);

            var insertInfo = new TransactionInfo
            {
                OperationType = OperationType.Insert,
                Table = tableName,
                Values = values,
                Fields = names
            };

            _transactionInfo.Add(insertInfo);
        }

        public List<DataProperty[]> Select(string tableName, Condition condition, string[] fields)
        {
            var result = new List<DataProperty[]>();
            string aggregatedFields = fields.Select(n => $"{n}").Aggregate((s1, s2) => s1 + "," + s2);

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    string query = $"SELECT {aggregatedFields} FROM {tableName} WHERE {condition.ToSql()}";
                    cmd.CommandText = query;
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int fieldsCount = reader.FieldCount;

                    while (reader.Read())
                    {
                        var record = new List<DataProperty>();
                        for (int i = 0; i < fieldsCount; i++)
                            record.Add(new DataProperty { Name = reader.GetName(i), Value = reader.GetValue(i) });

                        result.Add(record.ToArray());
                    }
                }
            }

            return result;
        }

        public void Delete(string tableName, Condition condition)
        {
            var deleteInfo = new TransactionInfo
            {
                Table = tableName,
                OperationType = OperationType.Delete,
                Condition = condition
            };

            _transactionInfo.Add(deleteInfo);
        }

        public void Update(string tableName, Condition condition, params DataProperty[] propertyValues)
        {
            string result = propertyValues.Select(pv => $"@{pv.Name}='{pv.Value}'").Aggregate((s1, s2) => s1 + "," + s2);

            var updateInfo = new TransactionInfo
            {
                OperationType = OperationType.Update,
                Table = tableName,
                Condition = condition,
                Values = result
            };

            _transactionInfo.Add(updateInfo);
        }

        public SqlTransaction BeginTransaction()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            foreach (TransactionInfo item in _transactionInfo)
            {
                switch (item.OperationType)
                {
                    case OperationType.Insert:
                        command.CommandText = $@"INSERT INTO {item.Table} ({item.Fields}) VALUES ({item.Values});";
                        foreach (char parameter in item.Values)
                            command.Parameters.Add(parameter);

                        command.ExecuteNonQuery();
                        break;

                    case OperationType.Delete:
                        command.CommandText = $@"DELETE FROM {item.Table} WHERE {item.Condition.ToSql()}";
                        command.ExecuteNonQuery();
                        break;

                    case OperationType.Update:
                        command.CommandText = $@"UPDATE {item.Table} SET {item.Values} WHERE {item.Condition.ToSql()}";
                        command.ExecuteNonQuery();
                        break;
                }
            }

            return transaction;
        }

        public void CommitTransaction(SqlTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public void RollbackTransaction(SqlTransaction transaction)
        {
            try
            {
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        #endregion
    }
}