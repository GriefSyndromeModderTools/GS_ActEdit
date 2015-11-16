using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    class PropertyTable
    {
        public class ExplicitTypeAttribute : Attribute
        {
            public ExplicitTypeAttribute(int value)
            {
                this.value = value;
            }

            public int value;
        }

        private class TypeHandler
        {
            public TypeHandler(TypeInfo field_type, string reader, string writer)
            {
                this.field_type = field_type;
                this.reader = reader;
                this.writer = writer;
            }

            public bool CheckField(PropertyInfo f)
            {
                return (f.PropertyType == field_type);
            }

            public void ReadField(object obj, PropertyInfo f, BinaryInputStream s)
            {
                f.SetValue(obj, typeof(BinaryInputStream).GetMethod(reader).Invoke(s, null));
            }

            public void WriteField(object obj, PropertyInfo f, BinaryOutputStream s)
            {
                typeof(BinaryOutputStream).GetMethod(writer).Invoke(s, new object[] { f.GetValue(obj) });
            }

            private TypeInfo field_type;
            private string reader;
            private string writer;
        }

        private static TypeHandler[] type_handlers;

        static PropertyTable()
        {
            type_handlers = new TypeHandler[] {
                new TypeHandler(typeof(Int32).GetTypeInfo(), "ReadInt32", "WriteInt32"),
                new TypeHandler(typeof(float).GetTypeInfo(), "ReadFloat", "WriteFloat"),
                new TypeHandler(typeof(bool).GetTypeInfo(), "ReadBool", "WriteBool"),
                new TypeHandler(typeof(string).GetTypeInfo(), "ReadString", "WriteString"),
                new TypeHandler(typeof(Int32).GetTypeInfo(), "ReadInt32", "WriteInt32"),
            };
        }

        private int GetTypeId(TypeInfo ti)
        {
            if (ti.Equals(typeof(Int32).GetTypeInfo()))
            {
                return 0;
            }
            if (ti.Equals(typeof(float).GetTypeInfo()))
            {
                return 1;
            }
            if (ti.Equals(typeof(bool).GetTypeInfo()))
            {
                return 2;
            }
            if (ti.Equals(typeof(string).GetTypeInfo()))
            {
                return 3;
            }
            return 0;
        }

        public void ReadFromStream(BinaryInputStream s, bool is_short_version = false)
        {
            List<Action<BinaryInputStream>> readers = new List<Action<BinaryInputStream>>();

            if (!is_short_version)
            {
                s.ReadBool(true);
            }

            int size = s.ReadInt32();
            for (int i = 0; i < size; ++i)
            {
                string n = s.ReadString();
                int t = s.ReadInt32();
                readers.Add(SetField(n, t));
            }
            foreach (var ac in readers)
            {
                ac(s);
            }
        }

        public void WriteToStream(BinaryOutputStream s, bool is_short_version = false)
        {
            if (!is_short_version)
            {
                s.WriteBool(true);
            }

            List<KeyValuePair<string, int>> name_list = new List<KeyValuePair<string, int>>();
            List<Action> write_desc = new List<Action>();
            List<Action> write_values = new List<Action>();
            foreach (PropertyInfo pi in this.GetType().GetTypeInfo().GetProperties())
            {
                string name = pi.Name;
                int type = GetTypeId(pi.PropertyType.GetTypeInfo());
                int raw_type = type;
                var atts = pi.GetCustomAttributes(typeof(ExplicitTypeAttribute));
                if (atts.Count() == 1)
                {
                    type = ((ExplicitTypeAttribute) atts.First()).value;
                }

                name_list.Add(new KeyValuePair<string, int>(name, name_list.Count));
                write_desc.Add(() =>
                {
                    s.WriteString(name);
                    s.WriteInt32(type);
                });
                write_values.Add(() =>
                {
                    type_handlers[raw_type].WriteField(this, pi, s);
                });
            }
            name_list.Sort((a, b) => a.Key.CompareTo(b.Key));

            s.WriteInt32(name_list.Count);
            foreach (var n in name_list)
            {
                write_desc[n.Value]();
            }
            foreach (var n in name_list)
            {
                write_values[n.Value]();
            }
        }

        private Action<BinaryInputStream> SetField(string name, int type)
        {
            TypeHandler th = type_handlers[type];
            PropertyInfo f = this.GetType().GetProperty(name);
            if (!th.CheckField(f))
            {
                throw new Exception("invalid field type in property table");
            }
            return (BinaryInputStream bs) => { th.ReadField(this, f, bs); };
        }
    }
}
