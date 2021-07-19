using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASPProject.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }
       
        [DisplayName("Insert animal name here:")]
        [Required(ErrorMessage = "Must insert a name.")]
        [StringLength(20, ErrorMessage = ("There is limit of 20 charecters"))]
        public string Name { get; set; }
       
        [DisplayName("Insert animal age here:")]
        [Required(ErrorMessage = "Must insert an age")]
        [Range (0,120,ErrorMessage =("Until 120..."))]
        public int Age { get; set; }

        public string PictureName { get; set; }
        
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImgFile{ get; set; }
     
        [DataType(DataType.MultilineText)]
        [DisplayName("Insert animal description here:")]
        [Required(ErrorMessage = "Must insert a description")]
        [StringLength(200,ErrorMessage =("There is limit of 200 charecters"))]
        public string Descrition { get; set; }
      
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
    }
}
