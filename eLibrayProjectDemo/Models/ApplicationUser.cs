using Microsoft.AspNetCore.Identity;

namespace eLibrayProjectDemo.Models
{
    public class ApplicationUser : IdentityUser
    {

        //remeber when we want to add new fields - remove the second migration file and then create it again
        // or using the add comand
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime CreateAt { get; set; }
    }
}
