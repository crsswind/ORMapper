using System;

namespace ObjectRelationalMapper
{
    public class DbColumnAttribute : Attribute
    {
        #region Constructors

        public DbColumnAttribute(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion
    }
}