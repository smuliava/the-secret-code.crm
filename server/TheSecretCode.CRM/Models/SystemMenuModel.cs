using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class SystemMenuModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
    }
}