using System.Collections.Generic;


namespace RPDB.Domain.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateInfo DateInfo { get; set; }
        public List<Character> Characters { get; set; }
    }
}
