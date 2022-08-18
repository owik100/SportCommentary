using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.Entities
{
    public class SportType
    {
        public int SportTypeID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Commentary> Commentaries { get; set; }
    }
}
