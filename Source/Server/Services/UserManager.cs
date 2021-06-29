using System.Collections.Generic;
using System.Linq;

namespace MultiplayerSnake.Server
{
    public class UserManager
    {
        private IList<User> _users;

        public UserManager()
        {
            _users = new List<User>();
        }

        public User GetUserByName(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public User GetUserById(string userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }
    }
}
