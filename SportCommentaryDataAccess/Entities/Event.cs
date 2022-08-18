using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.Entities
{
    public class Event
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<SingleComment> SingleComments { get; set; }
    }
}
