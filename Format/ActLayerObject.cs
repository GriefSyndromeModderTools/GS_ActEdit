using GS_ActEdit.Format.Serialization;
using GS_ActEdit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 649

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVCActLayer@@")]
    class ActLayerObject : ISerializable
    {
        public class Properties : PropertyTable
        {
            public bool debugOnly { get; set; }
            public float dst_x { get; set; }
            public float dst_y { get; set; }
            public float dst_z { get; set; }
            public bool expand { get; set; }
            public int layerID { get; set; }
            public float ox { get; set; }
            public float oy { get; set; }
            public float oz { get; set; }
            public int parentID { get; set; }
            public float prev_x { get; set; }
            public float prev_y { get; set; }
            public float prev_z { get; set; }
            public int resourceID { get; set; }
            public string stName { get; set; }
            public bool useKeyTimeline { get; set; }
            public bool visible { get; set; }
        }

        public Properties properties = new Properties();
        public List<ActKeyObject> keys = new List<ActKeyObject>();
        public CodeObject code = new CodeObject();

        public void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);
            keys = s.ReadObjectArray<ActKeyObject>();
            if (keys.Count != 1)
            {
                Task.SendError("layer should contain one key");
            }
            s.ReadInt32(0);
            code.Read(s);
        }

        public void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);
            s.WriteObjectArray(keys);
            s.WriteInt32(0);
            code.Write(s);
        }

        public static ActLayerObject CreateDefault(string name, int id, int resID)
        {
            ActLayerObject ret = new ActLayerObject();

            ret.properties.expand = true;
            ret.properties.layerID = id;
            ret.properties.parentID = -1;
            ret.properties.resourceID = resID;
            ret.properties.stName = name;
            ret.properties.useKeyTimeline = true;
            ret.properties.visible = true;

            ret.keys.Add(ActKeyObject.CreateDefault());
            return ret;
        }

        public Act2DMapLayoutObject GetLayout()
        {
            try
            {
                return (Act2DMapLayoutObject)keys[0].layout;
            }
            catch
            {
                return null;
            }
        }

        public void CalculateAABB(ActObject file)
        {
            var layout = GetLayout();
            if (layout != null)
            {
                layout.CalculateAABB(file);
            }
        }
    }
}
