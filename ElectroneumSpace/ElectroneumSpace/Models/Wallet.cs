using Realms;
using System;

namespace ElectroneumSpace.Models
{
    public class Wallet : RealmObject
    {

        [PrimaryKey]
        public string Address { get; set; }

        public string Nickname { get; set; }
        
        public DateTimeOffset Created { get; set; }

    }
}
