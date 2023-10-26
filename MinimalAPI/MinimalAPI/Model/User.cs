using Microsoft.EntityFrameworkCore.InMemory.Design.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalAPI.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public User (string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public User (int id, string Username, string Password)
        {
            this.Id = id;
            this.Username = Username;
            this.Password = Password;
        }
    }
}
