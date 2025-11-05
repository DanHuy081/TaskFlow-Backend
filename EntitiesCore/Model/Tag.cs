using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        [Column("TagId")]
        public string TagId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Color")]
        public string Color { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }
    }
}
