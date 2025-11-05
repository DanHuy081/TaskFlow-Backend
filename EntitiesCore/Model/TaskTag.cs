using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Model
{
    [Table("TaskTags")]
    public class TaskTag
    {
       
        public string TaskId { get; set; }

        
        public string TagId { get; set; }
    }
}
