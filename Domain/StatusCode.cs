using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
   public class StatusCode
    {
        [Key]
        public char Code { get; set; }
        public string Name { get; set; }
    }
}
