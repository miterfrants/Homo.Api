using System.ComponentModel.DataAnnotations;

namespace Homo.Api.Models
{
    public abstract partial class DTOs
    {
        public class Test
        {
            [Required(ErrorMessage = "請填寫名稱")]
            public string Name { get; set; }
        }
    }

}