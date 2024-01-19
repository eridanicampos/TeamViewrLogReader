using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamViewerLogReader.Domain
{
    public class UserLoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid UserTvLogId { get; set; }

        [StringLength(255)]
        public string ComputerName { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }

        [Required]
        public DateTime LoginTimestamp { get; set; }
        [StringLength(255)]
        public string MacAddress { get; set; }

        [ForeignKey("UserTvLogId")]
        public virtual UserTvLog UserTvLog { get; set; }

        
    }
}
