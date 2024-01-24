using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDummy.Model
{
    public class LoginResponse
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string image { get; set; }
        public string token { get; set; }
    }
}
