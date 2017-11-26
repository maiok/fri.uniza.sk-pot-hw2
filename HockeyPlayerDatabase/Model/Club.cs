using System;
using System.Collections.Generic;
using HockeyPlayerDatabase.Interfaces;

namespace HockeyPlayerDatabase.Model
{
    public class Club : IClub
    {
        public Club(string name, string address, string url)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public Club()
        {
            //throw new NotImplementedException();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Address}, {Url}";
        }

        public virtual List<Player> Players { get; set; }
    }
}
