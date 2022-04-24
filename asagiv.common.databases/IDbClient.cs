using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbClient
    {
        void Connect(string connectionString);
        Task ConnectAsync(string connectionString);
    }
}
