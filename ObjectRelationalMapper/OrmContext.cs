using System;
using System.Data.SqlClient;

namespace ObjectRelationalMapper
{
    public class OrmContext
    {
        #region Fields

        private readonly IDbProvider _ormProvider;

        #endregion

        #region Constructors

        public OrmContext(IDbProvider provider)
        {
            _ormProvider = provider;
        }

        #endregion

        #region  Public Methods

        public Repository<T> GetRepository<T>()
        {
            return new Repository<T>(_ormProvider);
        }

        public void SaveChanges()
        {
            SqlTransaction transactionResult = _ormProvider.BeginTransaction();

            try
            {
                _ormProvider.CommitTransaction(transactionResult);
            }
            catch (Exception)
            {
                _ormProvider.RollbackTransaction(transactionResult);
            }
        }

        #endregion
    }
}