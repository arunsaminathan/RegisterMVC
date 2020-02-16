using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegisterationMVC.Models
{
    public class Registeration
    {
        [Display(Name ="First Name")]
        [Required(ErrorMessage = "Please enter First Name")]
        [DataType(DataType.Text),MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Last Name")]
        [DataType(DataType.Text), MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Please enter Email Id")]
        [DataType(DataType.EmailAddress), MaxLength(50)]
        public string Email { get; set; }

        [Display(Name = "Confirm Email ID")]
        [Required(ErrorMessage = "Please enter Confrim Email ID")]
        [DataType(DataType.EmailAddress), MaxLength(50)]
        [Compare("Email")]
        public string ConfirmEmailId { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter Mobile Number")]
        [DataType(DataType.PhoneNumber), MaxLength(15)]
        public string MobileNumber { get; set; }

        [Display(Name = "City")]
        //[Required(ErrorMessage = "Please enter City")]
        public string City { get; set; }

        [Display(Name = "Bill")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.pdf)$", ErrorMessage = "Only Image files allowed.")]
        //[Required(ErrorMessage = "Please enter Bill")]
        public Byte[] Bill { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }

        public List<byte[]> FileData { get; set; }

        public HttpPostedFile BillPath { get; set; }
    }
}