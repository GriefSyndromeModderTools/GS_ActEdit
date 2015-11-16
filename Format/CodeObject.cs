using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    class CodeObject : ISerializable
    {
        public class Properties : PropertyTable
        {
            public bool compiled { get; set; }
            public string filePath { get; set; }
        }

        public Properties properties = new Properties() { filePath = "" };
        public string code = "";

        public void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);
            code = s.ReadString();
        }

        public void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);
            s.WriteNullEndString(code);
        }
    }
}
