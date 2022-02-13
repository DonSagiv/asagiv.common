namespace asagiv.common.databases
{
    public interface IDbDatabase
    {
        public string DatabaseName { get; }
        public IDbClient Client { get; }
    }
}
