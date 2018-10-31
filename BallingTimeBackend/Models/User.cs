using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallingTimeBackend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        //array of practice days in JSON
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string PracticeDays { get; set; }

        public ICollection<UserProgress> UserProgresses { get; set; }

        public User() {
            UserProgresses = new List<UserProgress>();
        }
    }
}
