using System;

namespace ObjectRelationalMapper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrmKeyAttribute : Attribute
    {
        #region Constructors

        public OrmKeyAttribute()
        {
            Order = 0;
        }

        public OrmKeyAttribute(int order)
        {
            Order = order;
        }

        #endregion

        #region Properties

        public int Order { get; set; }

        #endregion
    }
}