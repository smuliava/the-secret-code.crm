using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    [Table("tblMenus", Schema = "public")]
    public class Menu : Base
    {
        public Guid? ParentId { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Caption { get; set; }
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
        public string MenuType { get; set; }
    }
}