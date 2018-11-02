using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWeb.Models
{
    public class BookModel
    {
       
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  {get;set;}
        [Required]
        [MaxLength(30)]
      
        [Display(Name="書名")]
        public string BookName {get;set;}

        [Display(Name="價格")]
         public int Price {get;set;}
         
         [Display(Name="作者")]
        public string Author {get;set;}
        
        [Required]
        [Display(Name="出版社")]
        public string PublishingHouse {get;set;}
       [Display(Name="備註")]
        public string Remark {get;set;}
    }
}