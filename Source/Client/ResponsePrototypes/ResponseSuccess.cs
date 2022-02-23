using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    internal class Data
    {
        public string JwtToken { get; set; }
    }
    class ResponseSuccess
    {
        public Data Data { get; set; }
        public int StatusCode { get; set; }
    }
}
