using StackExchange.Redis;

namespace RestApi.Test.Repositories
{
    public class BaseRedisRepositoryTest
    {

        internal readonly IConnectionMultiplexer _redisConnect;

        public BaseRedisRepositoryTest()
        {
            _redisConnect = ConnectionMultiplexer.Connect(
                    new ConfigurationOptions
                    {
                        EndPoints = { "localhost:6379" }
                    });
        }
    }
}