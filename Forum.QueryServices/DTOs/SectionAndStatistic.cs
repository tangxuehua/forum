using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.QueryServices.DTOs
{
    public class SectionAndStatistic : SectionInfo
    {
        public int PostCount { get; set; }
        public int ReplyCount { get; set; }
    }
}
