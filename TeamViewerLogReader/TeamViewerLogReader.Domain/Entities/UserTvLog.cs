using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Domain
{

    public class UserTvLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Surname { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }

        public UserTvLog()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            IsDeleted = false;
        }
    }

}
