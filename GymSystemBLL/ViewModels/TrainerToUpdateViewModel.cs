using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels
{
    internal class TrainerToUpdateViewModel
    {
        public string Name { get; set; }
       

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be Less Between 5 and 100 Char !")]
        public string Email { get; set; } = null!;


        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(010|011|012|015)\d{8}$", ErrorMessage = "Phone Must Be Valid Egyptian Number  !")]
        public string Phone { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required !")]
        [Range(1, 9000, ErrorMessage = "BuildingNumber Must Be Greater than 0 !")]
        public int BuildingNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required !")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 !")]
        public string Street { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required !")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required !")]

        public Specialites Specialites { get; set; }
    }
}
