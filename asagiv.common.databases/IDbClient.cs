using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbClient
    {
        #region Methods
        void Connect(string connectionString);
        Task ConnectAsync(string connectionString);
        #endregion
    }
}
