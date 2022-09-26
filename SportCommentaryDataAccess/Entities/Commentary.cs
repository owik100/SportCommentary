using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.Entities
{
    public class Commentary
    {
        public int CommentaryID { get; set; }
        public string Caption { get; set; }
        public string? Description { get; set; }
        public string? CommentaryImage { get; set; }
        public DateTime CommentaryStart { get; set; }
        public bool IsLive { get; set; }
        [ForeignKey("SportType")]
        public int SportTypeID { get; set; }
    }
}
