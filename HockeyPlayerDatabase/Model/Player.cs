using System;
using HockeyPlayerDatabase.Interfaces;

namespace HockeyPlayerDatabase.Model
{
    public class Player : IPlayer
    {
        public Player(string firstName, string lastName, string titleBefore, int yearOfBirth, int krpId, AgeCategory? ageCategory, int? clubId)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            TitleBefore = titleBefore ?? throw new ArgumentNullException(nameof(titleBefore));
            YearOfBirth = yearOfBirth;
            KrpId = krpId;
            AgeCategory = ageCategory;
            ClubId = clubId;
        }

        public Player()
        {
            throw new NotImplementedException();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public string TitleBefore { get; set; }
        public int YearOfBirth { get; set; }
        public int KrpId { get; set; }
        public AgeCategory? AgeCategory { get; set; }
        public int? ClubId { get; set; }
        public virtual Club Club { get; set; }

        public override string ToString()
        {
            return $"{FullName}, {TitleBefore}, {YearOfBirth}, {KrpId}, {AgeCategory}, {ClubId}";
        }
    }
}
