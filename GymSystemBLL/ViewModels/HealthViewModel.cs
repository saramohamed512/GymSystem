using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels
{
    public class HealthViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required! ")]
        [Range(1, 300, ErrorMessage = "Height Must Be Greater than  0cm !")]
        public decimal Height { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required! ")]
        [Range(1, 500, ErrorMessage = "Weight Must Be Greater than  0 and Less Than 500!")]
        public decimal Weight { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required! ")]
        [StringLength(3, ErrorMessage = "Blood Type Must Be Between 3 Chars or Less !")]
        public string BloodType { get; set; }
        public string? Note { get; set; }

    }
}
