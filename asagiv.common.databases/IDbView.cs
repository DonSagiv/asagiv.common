using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbView
    {
        IDbDatabase Database { get; }
        string ViewName { get; }
    }

    public interface IDbView<TDbModel, TDatabaseIdentifier> : IDbView
        where TDbModel : class, IDbModel<TDatabaseIdentifier>
    {
        Task<TDbModel?> ReadAsync(TDatabaseIdentifier id);
        IAsyncEnumerable<TDbModel?> GetEnumerable(IEnumerable<TDatabaseIdentifier> idList, CancellationToken cancellationToken = default);
    }
}
