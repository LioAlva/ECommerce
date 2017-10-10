using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ECommerce.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [MaxLength(256, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "E-mAIL")]
        [Index("User_UserName_Index", 2, IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string    UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [MaxLength(20, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered")]
        [MaxLength(100, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]

        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        //[NotMapped]
        [Display(Name = "User")]
        public string FullName { get { return string.Format("{0} {1}",FirstName,LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }
    }
}