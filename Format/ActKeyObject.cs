using GS_ActEdit.Format.Serialization;
using GS_ActEdit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVCActKey@@")]
    class ActKeyObject : ISerializable
    {
        public class Properties : PropertyTable
        {
            public int activeLength { get; set; }
            public int beginFrame { get; set; }
            public float layerX { get; set; }
            public float layerY { get; set; }
            public float layerZ { get; set; }
            public int resourceID { get; set; }
            public string scriptFunction { get; set; }
        }

        public Properties properties = new Properties();
        public AbstractActLayoutObject layout;

        public void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);
            if (s.ReadBool() == false)
            {
                Task.SendError("key should contain one layout");
            }
            layout = s.ReadObject<AbstractActLayoutObject>();
            if (layout == null || !(layout is Act2DMapLayoutObject))
            {
                Task.SendError("only support Act2DMapLayout in key");
            }
        }

        public void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);
            if (layout == null)
            {
                s.WriteBool(false);
                return;
            }
            s.WriteBool(true);
            s.WriteObject(layout);
        }

        public static ActKeyObject CreateDefault()
        {
            ActKeyObject ret = new ActKeyObject();

            ret.properties.activeLength = 1;
            ret.properties.resourceID = -1;
            ret.properties.scriptFunction = "";

            ret.layout = Act2DMapLayoutObject.CreateDefault();

            return ret;
        }
    }
}
