using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    [Table("tblDictionaryValues", Schema = "public")]
    public class DictionaryValue : Dictionary
    {
        [Required]
        public Guid DictionaryNameId { get; set; }
        [Required]
        [StringLength(64)]
        public string Value { get; set; }
    }
}