using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVTextureResourceInfo@@")]
    class ActTextureResourceInfoObject : AbstractResourceInfoObject
    {
        public class Properties : PropertyTable
        {
            [PropertyTable.ExplicitType(4)]
            public int resourceID { get; set; }
            public string stFilePath { get; set; }
        }

        public Properties properties = new Properties();

        public override void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s, true);
        }

        public override void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s, true);
        }

        public override int GetId()
        {
            return properties.resourceID;
        }

        public override string GetDisplayName()
        {
            return Path.GetFileName(properties.stFilePath);
        }
    }
}
