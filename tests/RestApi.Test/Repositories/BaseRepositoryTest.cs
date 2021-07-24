using DbEntity;
using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

namespace RestApi.Test.Repositories
{
    public class BaseRepositoryTest
    {
        internal readonly DbContextEntity _context;

        public BaseRepositoryTest()
        {
            var connectionString = "Server=172.20.0.2; Port=3306;User Id=root;Password=Passwo!rd123!;Database=School";
            var options = new DbContextOptionsBuilder<DbContextEntity>()
                .UseMySql(
                   connectionString,
                    ServerVersion.AutoDetect(connectionString)
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;
                
            _context = new DbContextEntity(options);
        }
    }
}