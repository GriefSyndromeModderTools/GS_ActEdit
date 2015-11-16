using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.Serialization
{
    class BinaryOutputStream
    {
        public BinaryOutputStream(BinaryWriter bw)
        {
            this.bw = bw;
        }

        public void WriteInt16(short val)
        {
            bw.Write(val);
        }

        public void WriteInt32(Int32 val)
        {
            bw.Write(val);
        }

        public void WriteFloat(float val)
        {
            bw.Write(val);
        }

        public void WriteBool(bool val)
        {
            bw.Write(val);
        }

        public void WriteString(string str)
        {
            WriteInt32(str.Length);
            byte[] data = str.ToCharArray().Select(cc => (byte) cc).ToArray();
            bw.Write(data);
        }

        public void WriteNullEndString(string str)
        {
            string cstr = str;
            if (cstr.Length == 0 || cstr.Last() != '\0')
            {
                cstr += '\0';
            }
            WriteString(cstr);
        }

        private BinaryWriter bw;
    }
}
