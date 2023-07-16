using System.ComponentModel.DataAnnotations;

namespace MVC_News.Models
{
    public class LoginViewModel
    {
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
