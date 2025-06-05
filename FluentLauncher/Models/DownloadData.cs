using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentLauncher.Models
{
    public class DownloadData
    {
        public string gameVersion { get; set; }
        public string gamedescribe { get; set; }
        public List<ModLoaders> modLoaders { get; set; }
        public string type { get; set; }
    }
}
