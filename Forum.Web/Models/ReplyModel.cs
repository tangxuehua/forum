using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Web.Models
{
    public class ReplyModel
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CreatedOn { get; set; }
        public int Floor { get; set; }
    }
}