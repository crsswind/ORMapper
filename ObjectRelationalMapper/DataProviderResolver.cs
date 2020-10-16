namespace ObjectRelationalMapper
{
    internal static class DataProviderResolver
    {
        #region  Public Methods

        public static IDbProvider GetDbProvider()
        {
            return new SqlDataProvider();
        }

        #endregion
    }
}