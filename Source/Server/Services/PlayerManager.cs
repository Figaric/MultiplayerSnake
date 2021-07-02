using System.Collections.Generic;
using System.Linq;

namespace MultiplayerSnake.Server
{
    public class PlayerManager
    {
        private readonly IList<Player> _players;

        public PlayerManager()
        {
            _players = new List<Player>();
        }

        public Player GetPlayerByUserId(int userId)
        {
            return _players.FirstOrDefault(p => p.UserId == userId);
        }

        public Player GetPlayerById(string playerId)
        {
            return _players.FirstOrDefault(p => p.Id == playerId);
        }

        public bool AddPlayer(Player player)
        {
            _players.Add(player);

            return true;
        }
    }
}
