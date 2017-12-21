using Realms;
using System;

namespace ElectroneumSpace.Models
{
    public class Wallet : RealmObject
    {

        public string Nickname { get; set; }

        public string Address { get; set; }

        public DateTimeOffset Created { get; set; }

    }
}
