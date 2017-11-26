using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HockeyPlayerDatabase.Interfaces;

namespace HockeyPlayerDatabase.Model
{
    public class Club : IClub
    {
        public Club(int id, string name, string address, string url)
        {
            Id = id;
            Name = name;
            Address = address;
            Url = url;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Address}, {Url}";
        }
    }
}
