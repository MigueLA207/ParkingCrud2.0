
using System.ComponentModel.DataAnnotations;
namespace CrudPark.Application.DTOs
{
    public class UpdateMembershipDto
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}