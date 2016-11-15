namespace DAL.Base
{
    public interface IRepositoryBase
    {
        WfCustomDatabaseContext Context { get; }
    }
}