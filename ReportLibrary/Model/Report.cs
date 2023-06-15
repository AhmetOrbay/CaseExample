using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Model
{
    public class Report
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public ReportStatus Status { get; set; }
    }

    public enum ReportStatus
    {
        Waiting=0,
        Completed
    }
}
