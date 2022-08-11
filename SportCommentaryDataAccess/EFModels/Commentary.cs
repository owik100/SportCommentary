﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.EFModels
{
    public class Commentary
    {
        public int CommentaryID { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime CommentaryStart { get; set; }
    }
}