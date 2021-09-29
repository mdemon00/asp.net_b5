using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker.Models
{
    public class WorkerSettingsModel
    {
        public int Worker_Delay_Time { get; set; }
        public string Upload_Location { get; set; }
        public string Download_Location { get; set; }
    }
}
