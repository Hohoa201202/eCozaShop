using System.Threading.Tasks;

namespace eCozaStore.Models
{

    //Session
    public class User
    {

        public int? CustomerID { get; set; }
        public string? FullName { get; set; }
        public string? Avata { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
