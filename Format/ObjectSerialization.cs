using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    static class ObjectSerialization
    {
        public class IDAttribute : Attribute
        {
            public IDAttribute(string id)
            {
                this.id = id;
            }

            public int Hash()
            {
                return CalcHash(id);
            }

            public static int CalcHash(string str)
            {
                UInt32 hash = 0;
                foreach (char c in str)
                {
                    UInt32 ic = (UInt32)c;
                    hash ^= (ic + (hash << 6) + (hash >> 2) + 0x9E3779B9);
                }
                return (int) hash;
            }

            private string id;
        }

        static ObjectSerialization()
        {
            AddClass<ActObject>();
            AddClass<ActLayerObject>();
            AddClass<ActKeyObject>();
            AddClass<Act2DMapLayoutObject>();
            AddClass<ActResourceChipObject>();
            AddClass<ActTextureResourceInfoObject>();
            AddClass<MeshResourceInfoObject>();
        }

        private static int GetIDForClass(TypeInfo ti)
        {
            var attr = ti.GetCustomAttributes(typeof(IDAttribute), false);
            if (attr.Length != 1 || ! (attr[0] is IDAttribute))
            {
                throw new Exception("invalid object class");
            }
            IDAttribute id = (IDAttribute)attr[0];
            return id.Hash();
        }

        private static void AddClass<T>() where T : ISerializable, new()
        {
            creators.Add(GetIDForClass(typeof(T).GetTypeInfo()), () => new T());
        }

        public static T ReadObject<T>(this BinaryInputStream bs) where T : class, ISerializable
        {
            int classid = bs.ReadInt32();
            if (!creators.ContainsKey(classid))
            {
                throw new Exception("unknown classid");
            }
            ISerializable ser = creators[classid]();
            if (!(ser is T))
            {
                throw new Exception("incorrect object type in stream");
            }
            T ret = (T) ser;
            ret.Read(bs);
            return ret;
        }

        public static List<T> ReadObjectArray<T>(this BinaryInputStream s) where T : ISerializable
        {
            return s.ReadSerializableArray<T>(s.ReadInt32(), bs => {
                int id = s.ReadInt32();
                return (T)creators[id]();
            });
        }

        public static void WriteObject<T>(this BinaryOutputStream bs, T obj) where T : class, ISerializable
        {
            bs.WriteInt32(GetIDForClass(obj.GetType().GetTypeInfo()));
            obj.Write(bs);
        }

        public static void WriteObjectArray<T>(this BinaryOutputStream s, List<T> list) where T : class, ISerializable
        {
            s.WriteInt32(list.Count);
            s.WriteSerializableArray<T>(list, (bs, i) => { bs.WriteInt32(GetIDForClass(i.GetType().GetTypeInfo())); });
        }

        private static Dictionary<int, Func<ISerializable>> creators = new Dictionary<int, Func<ISerializable>>();
    }
}
