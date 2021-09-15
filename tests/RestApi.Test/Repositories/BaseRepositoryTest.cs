using DbEntity;
using Microsoft.EntityFrameworkCore;

namespace RestApi.Test.Repositories
{
    public class BaseRepositoryTest
    {
        internal readonly DbContextEntity _context;

        public BaseRepositoryTest()
        {
            var connectionString = "Server=localhost; Port=3306;User Id=newuser;Password=Passwo!rd123!;Database=Furniture";
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