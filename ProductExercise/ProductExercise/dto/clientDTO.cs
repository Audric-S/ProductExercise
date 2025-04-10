using System;
using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class ClientDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
