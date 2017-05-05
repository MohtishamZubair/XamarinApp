using SQLite;
using System;

namespace SalesAgentDistribution.Model
{
    public class BGActivity
    {
        [PrimaryKey, AutoIncrement]
        public int ActivityId { get; set; }
        public string ActivityActionName { get; set; }
        public string TargetObjectJSON { get; set; }
        public int TargetObjectId { get; set; }
        public DateTime CreatedDate { get; internal set; }
        public string ServiceURL { get; internal set; }
    }
}