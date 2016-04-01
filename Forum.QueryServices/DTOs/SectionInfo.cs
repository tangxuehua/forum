using System;
using System.Collections.Generic;

namespace Forum.QueryServices.DTOs
{
    /// <summary>表示一个版块的信息
    /// </summary>
    public class SectionInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SectionInfo()
        {
            Id = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
