using System;

namespace ObjectRelationalMapper
{
    public class DbTableAttribute : Attribute
    {
        #region Constructors

        public DbTableAttribute(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion
    }
}