using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class SystemMenuModel
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public Guid ParrentId { get; set; }
        public Guid CreatedBySystemUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBySystemUserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Caption { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}