using System;

namespace ObjectRelationalMapper
{
    public class ActionResultException : Exception
    {
        #region Properties

        public new string Message { get; set; }

        public new Exception InnerException { get; set; }

        #endregion
    }
}