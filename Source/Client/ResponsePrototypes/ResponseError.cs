using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Client
{
    internal class SingleError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
    class ResponseError
    {
        public List<SingleError> Errors { get; set; }
        public int StatusCode { get; set; }
    }
}
