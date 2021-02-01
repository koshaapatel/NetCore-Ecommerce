using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetcoreProject2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public decimal Rating { get; set; }
        public string Image { get; set; }
        public string CommentDesc { get; set; }
    }
}
