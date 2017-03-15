using System.Collections.Generic;

namespace RPDB.Domain.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EyeColor { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string SkinColor { get; set; }
        public string Race { get; set; }
        public string Faction { get; set; }
        public string Background { get; set; }
        public string Residence { get; set; }
        public string Occupation { get; set; }
        public string PhysicalAppearance { get; set; }
        public List<string> Friends { get; set; }
        public List<Story> Stories{ get; set; }
        public List<Picture> Pictures { get; set; }
        public Guild Guild { get; set; }
        public DateInfo DateInfo { get; set; }
    }
}