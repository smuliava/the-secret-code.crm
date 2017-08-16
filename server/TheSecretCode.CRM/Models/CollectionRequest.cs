using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Models
{
    public class CollectionRequest<TCollection> : Request
    {
        public bool Reorder { get; set; }
        public TCollection[] Collection { get; set; }
    }
}