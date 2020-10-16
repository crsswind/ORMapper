using LogicalExpressionGenerator;

namespace ObjectRelationalMapper
{
    public class TransactionInfo
    {
        #region Properties

        public string Table { get; set; }

        public string Fields { get; set; }

        public string Values { get; set; }

        public Condition Condition { get; set; }

        public OperationType OperationType { get; set; }

        #endregion
    }
}