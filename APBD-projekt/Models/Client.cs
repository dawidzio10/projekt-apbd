using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_projekt.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClient { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
        [Required]
        [MaxLength(300)]
        public string Password { get; set; }
        [Required]
        [MaxLength(300)]
        public string Salt { get; set; }
        [Required]
        [MaxLength(300)]
        public string RefreshToken { get; set; }
        [Required]
        public DateTime TokenExpirationDate { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}