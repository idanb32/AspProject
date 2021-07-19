using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject.Models
{
    public class Comment
    {
        [Key]
        public int CommentId{ get; set; }
        [ForeignKey("Animals")]    
        public int AnimalId { get; set; }
        public string CommentData{ get; set; }
    }
}
