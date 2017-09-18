using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CreatedBySystemUserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBySystemUserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedOn { get; set; }
    }
}