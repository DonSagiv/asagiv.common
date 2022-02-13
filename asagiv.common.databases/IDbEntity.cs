using System.Threading.Tasks;

namespace asagiv.common.databases
{
    public interface IDbEntity<TDbModel, TDatabaseIdentifier>
        where TDbModel : class, IDbModel<TDatabaseIdentifier>
    {
        #region Properties
        string? Label { get; set; }
        TDatabaseIdentifier Id { get; }
        #endregion

        #region Methods
        Task<TDbModel?> GetModelAsync();
        #endregion
    }
}
