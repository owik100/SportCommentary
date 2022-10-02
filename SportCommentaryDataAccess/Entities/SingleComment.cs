using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.Entities
{
    public class SingleComment
    {
        public int SingleCommentID { get; set; }
        [ForeignKey("Commentary")]
        public int CommentaryID { get; set; }
        [ForeignKey("Event")]
        public int? EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }

        public virtual Event? Event { get; set; }
        public virtual Commentary Commentary { get; set; }

    }
}
