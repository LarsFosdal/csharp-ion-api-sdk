using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.OAuth2SampleConsoleResourceOwner
{

    public class MMS200MI
    {
        public string Program { get; set; }
        public string Transaction { get; set; }
        public Metadata Metadata { get; set; }
        public Mirecord[] MIRecord { get; set; }
    }

    public class Metadata
    {
        public Field[] Field { get; set; }
    }

    public class Field
    {
        public string name { get; set; }
        public string type { get; set; }
        public string length { get; set; }
        public string description { get; set; }
    }

    public class Mirecord
    {
        public string RowIndex { get; set; }
        public Namevalue[] NameValue { get; set; }
    }

    public class Namevalue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
