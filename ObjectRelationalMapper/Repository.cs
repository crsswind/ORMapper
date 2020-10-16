using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogicalExpressionGenerator;

namespace ObjectRelationalMapper
{
    public class Repository<TEntity>
    {
        #region Fields

        private readonly PropertyInfo[] _keyProperties;

        private readonly IDbProvider _provider;

        private List<TransactionInfo> _transactionInfo;

        #endregion

        #region Constructors

        public Repository()
            : this(DataProviderResolver.GetDbProvider())
        {
        }

        public Repository(IDbProvider provider)
        {
            _transactionInfo = new List<TransactionInfo>();
            _provider = provider;
            _keyProperties = GetKeyProperties(typeof(TEntity));
        }

        #endregion

        #region Properties

        public string ConnectionString { get; set; }

        #endregion

        #region  Public Methods

        public object FormatValue(Type property, object value)
        {
            object result;

            switch (property.Name)
            {
                case "String":
                    return $"'{value}'";

                case "DateTime":
                    result = $"'{value}'";
                    break;
                case "Char":
                    result = $"'{value}'";
                    break;

                default:
                    result = value;
                    break;
            }

            return result;
        }

        public void Insert(TEntity entity)
        {
            Type type = typeof(TEntity);
            DataProperty[] propertyValues = typeof(TEntity).GetProperties()
                                                           .Where(p => p.GetCustomAttributes<DbColumnAttribute>().Any())
                                                           .Select(p => new DataProperty {
                                                                                             Name = p.GetCustomAttributes<DbColumnAttribute>().FirstOrDefault()?.Name,
                                                                                             Value = p.GetValue(entity)
                                                                                         }).ToArray();

            _provider.Insert(GetTableName(), propertyValues);
        }

        public void Delete(params object[] keys)
        {
            IEnumerable<ComparativeOperator<string>> comparisons = _keyProperties.Select((it, i) =>
                                                                                             new ComparativeOperator<string>(ComparativeOperatorType.Equal,
                                                                                                                             new StringVariable(it.GetCustomAttribute<DbColumnAttribute>().Name),
                                                                                                                             new StringVariable(keys[i].ToString())));

            if (keys.Length == 1)
            {
                Delete(comparisons.First());
            }
            else
            {
                Condition c = comparisons.Skip(2).Aggregate(new BinaryLogical(BinaryLogicalType.And, comparisons.First(), comparisons.Skip(1).First())
                                                          , (cond, comparison) => new BinaryLogical(BinaryLogicalType.And, cond, comparison));
                Delete(c);
            }
        }

        public void Delete(TEntity entity)
        {
            Delete((from prop in _keyProperties
                    select prop.GetValue(entity)).ToArray());
        }

        public void Delete(Condition condition)
        {
            _provider.Delete(GetTableName(), condition);
        }

        public List<DataProperty[]> Select(string[] fields, params object[] keys)
        {
            IEnumerable<ComparativeOperator<string>> comparisons = _keyProperties.Select((it, i) =>
                                                                                             new ComparativeOperator<string>(ComparativeOperatorType.Equal,
                                                                                                                             new StringVariable(it.GetCustomAttribute<DbColumnAttribute>().Name),
                                                                                                                             new StringVariable(keys[i].ToString())));
            if (keys.Length == 1)
                return Select(comparisons.First(), fields);

            Condition c = comparisons.Skip(2).Aggregate(new BinaryLogical(BinaryLogicalType.And, comparisons.First(), comparisons.Skip(1).First())
                                                      , (cond, comparison) => new BinaryLogical(BinaryLogicalType.And, cond, comparison));

            return Select(c, fields);
        }

        public List<DataProperty[]> Select(Condition condition, string[] fields)
        {
            return _provider.Select(GetTableName(), condition, fields);
        }

        public void Update(DataProperty[] propertyValues, params object[] keys)
        {
            IEnumerable<ComparativeOperator<string>> comparisons = _keyProperties.Select((it, i) =>
                                                                                             new ComparativeOperator<string>(ComparativeOperatorType.Equal, new StringVariable(it.Name),
                                                                                                                             new StringVariable(keys[i].ToString())));

            if (keys.Length == 1)
            {
                Update(comparisons.First(), propertyValues);
            }

            else
            {
                Condition c = comparisons.Skip(2).Aggregate(new BinaryLogical(BinaryLogicalType.And, comparisons.First(), comparisons.Skip(1).First())
                                                          , (cond, comparison) => new BinaryLogical(BinaryLogicalType.And, cond, comparison));
                Update(c, propertyValues);
            }
        }

        public void Update(Condition condition, params DataProperty[] propertyValues)
        {
            _provider.Update(GetTableName(), condition, propertyValues);
        }

        #endregion

        #region Protected Methods

        protected virtual void OnRowAdded(EventArgs e)
        {
            RowAdded?.Invoke(this, e);
        }

        protected virtual void OnRowDeleted(EventArgs e)
        {
            RowDeleted?.Invoke(this, e);
        }

        protected virtual void OnRowUpdated(EventArgs e)
        {
            RowUpdated?.Invoke(this, e);
        }

        protected virtual void OnRowSelected(EventArgs e)
        {
            RowSelected?.Invoke(this, e);
        }

        #endregion

        #region Private Methods

        private static PropertyInfo[] GetKeyProperties(Type type)
        {
            return (from p in type.GetProperties()
                    let att = p.GetCustomAttribute<OrmKeyAttribute>()
                    where att != null
                    orderby att.Order
                    select p
                   ).ToArray();
        }

        private static string GetTableName()
        {
            return typeof(TEntity).GetCustomAttributes<DbTableAttribute>().First().Name;
        }

        #endregion

        #region Events

        public event EventHandler RowAdded;

        public event EventHandler RowDeleted;

        public event EventHandler RowUpdated;

        public event EventHandler RowSelected;

        #endregion
    }
}