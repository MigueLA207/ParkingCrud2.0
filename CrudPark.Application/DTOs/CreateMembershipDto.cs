
using System.ComponentModel.DataAnnotations; 

namespace CrudPark.Application.DTOs
{
    public class CreateMembershipDto
    {

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? CustomerEmail { get; set; }


        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(50)]
        public string VehicleType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}