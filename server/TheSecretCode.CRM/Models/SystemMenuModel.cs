using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class SystemMenuModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Caption { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}