using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 649

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVCActResourceChip@@")]
    class ActResourceChipObject : AbstractActResourceObject
    {
        public class Properties : PropertyTable
        {
            public int resourceID { get; set; }
            public string stChipFile { get; set; }
            public string stName { get; set; }
        }

        public Properties properties = new Properties();

        public override void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);
        }

        public override void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);
        }

        public override string GetDisplayName()
        {
            return properties.stName;
        }
    }
}
