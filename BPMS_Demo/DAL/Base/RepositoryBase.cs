namespace DAL.Base
{
    public class RepositoryBase : IRepositoryBase
    {
        private WfCustomDatabaseContext _context;

        public WfCustomDatabaseContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new WfCustomDatabaseContext();
                }

                return _context;
            }
        }
    }
}