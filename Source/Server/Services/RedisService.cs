using StackExchange.Redis;

namespace MultiplayerSnake.Server;

public class RedisService
{
    private IConnectionMultiplexer _redis;

    public RedisService()
    {
        _redis = ConnectionMultiplexer.Connect("localhost");
    }

    public async Task<string> GenerateForgotPasswordTokenAsync(int userId)
    {
        var database = _redis.GetDatabase();
        string token = Guid.NewGuid().ToString();

        await database.StringSetAsync("forgot_password:" + token, userId);

        return token;
    }
}