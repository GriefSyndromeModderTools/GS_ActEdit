using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.Serialization
{
    class BinaryInputStream
    {
        public BinaryInputStream(BinaryReader br)
        {
            b = br;
        }

        public short ReadInt16()
        {
            return b.ReadInt16();
        }

        public int ReadInt32()
        {
            return b.ReadInt32();
        }

        public float ReadFloat()
        {
            return b.ReadSingle();
        }

        public bool ReadBool()
        {
            return b.ReadBoolean();
        }

        public string ReadString()
        {
            byte[] ba = b.ReadBytes(ReadInt32());
            int zero = Array.FindIndex(ba, bb => bb == 0);
            if (zero == -1)
            {
                zero = ba.Length;
            }
            return new string(ba.Select(bb => (char)bb).ToArray(), 0, zero);
        }

        private readonly BinaryReader b;
    }
}
