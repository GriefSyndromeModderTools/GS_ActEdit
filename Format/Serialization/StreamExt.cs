using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.Serialization
{
    static class StreamExt
    {
        public static void ReadInt16(this BinaryInputStream bs, short expect)
        {
            if (bs.ReadInt16() != expect)
            {
                throw new Exception("expecting " + expect + " in stream");
            }
        }
        public static void ReadInt32(this BinaryInputStream bs, int expect)
        {
            if (bs.ReadInt32() != expect)
            {
                throw new Exception("expecting " + expect + " in stream");
            }
        }

        public static void ReadBool(this BinaryInputStream bs, bool expect)
        {
            if (bs.ReadBool() != expect)
            {
                throw new Exception("expecting " + expect + " in stream");
            }
        }

        public static List<T> ReadSerializableArray<T>(this BinaryInputStream bs, int size, Func<BinaryInputStream, T> creator) where T : ISerializable
        {
            List<T> ret = new List<T>(size);
            for (int i = 0; i < size; ++i)
            {
                T e = creator(bs);
                e.Read(bs);
                ret.Add(e);
            }
            return ret;
        }

        public static void WriteSerializableArray<T>(this BinaryOutputStream bs, List<T> list, Action<BinaryOutputStream, T> header) where T : ISerializable
        {
            foreach (var i in list)
            {
                header(bs, i);
                i.Write(bs);
            }
        }
    }
}
