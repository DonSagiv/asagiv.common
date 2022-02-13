using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbClient
    {
        Task ConnectAsync(string connectionString);
    }
}
