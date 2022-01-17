using System.Collections.Generic;
using System.Linq;

namespace MultiplayerSnake.Server
{
    public class PlayerManager
    {
        private IList<Player> _players;

        public PlayerManager()
        {
            _players = new List<Player>();
        }

        /// <summary>
        /// Gets player by id
        /// </summary>
        /// <param name="playerId">The id of the player u wanna find</param>
        /// <returns>Null if not found, otherwise player instance</returns>
        public Player GetPlayerById(string playerId)
        {
            var player = _players.FirstOrDefault(p => p.Id == playerId);

            return player;
        }

        public IList<Player> GetPlayersByCurrentRoom(string roomName) 
            => _players.Where(p => p.CurrentRoomName == roomName).ToList();

        public bool AddPlayer(string id, string roomName = null)
        {
            var player = new Player
            {
                Id = id,
                CurrentRoomName = roomName
            };

            _players.Add(player);

            return true;
        }
    }
}
