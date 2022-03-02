using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Shared
{
    public class Player
    {
        public string Nickname { get; set; }

        public bool IsHost { get; set; }

        // In-Game props
        public IList<Point> SParts { get; set; }
        public ColorManager ColorManager { get; set; }
    }
}
