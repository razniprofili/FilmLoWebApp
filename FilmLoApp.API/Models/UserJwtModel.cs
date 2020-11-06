using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Models
{
    //trenutno ulogovan user Model
    public class UserJwtModel
    {
        public long Id { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; } //kada istice nas token
    }
}
