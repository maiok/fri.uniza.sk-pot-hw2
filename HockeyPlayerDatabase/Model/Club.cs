using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using HockeyPlayerDatabase.Interfaces;
using HockeyPlayerDatabase.Migrations;

namespace HockeyPlayerDatabase.Model
{
    public class Club : IClub
    {
        public Club(string name, string address, string url)
        {
            Name = name;
            Address = address;
            Url = url;
        }

        public Club()
        {
            //throw new NotImplementedException();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Address}, {Url}";
        }

        // Vytvorenie vztahu 1:N s hracmi
        public virtual List<Player> Players { get; set; }
    }
}
