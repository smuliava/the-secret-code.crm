using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    [Table("tblMenu", Schema = "public")]
    public class SystemMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid CreatedBySystemUserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBySystemUserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedOn { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Caption { get; set; }
        public string Title { get; set; }
        [Required]
        public int Order { get; set; }
        public string MenuType { get; set; }
    }
}