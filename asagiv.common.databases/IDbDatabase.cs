namespace asagiv.common.databases
{
    public interface IDbDatabase
    {
        #region Properties
        string DatabaseName { get; }
        IDbClient Client { get; }
        #endregion
    }
}