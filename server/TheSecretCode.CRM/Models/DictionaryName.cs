using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    [Table("tblDictionaryNames", Schema = "public")]
    public class DictionaryName : Dictionary
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; }
    }
}