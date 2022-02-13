using System.Collections.Generic;
using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbCollection<TDbModel, TDatabaseIdentifier>
        : IDbView<TDbModel, TDatabaseIdentifier>
        where TDbModel : class, IDbModel<TDatabaseIdentifier>
    {
        public Task AppendAsync(TDbModel modelToAdd);
        public Task<TDbModel?> DeleteAsync(TDatabaseIdentifier id);
    }
}
