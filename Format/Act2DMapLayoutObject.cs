using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVC2DMapLayout@@")]
    class Act2DMapLayoutObject : AbstractActLayoutObject
    {
        public class Properties : PropertyTable
        {
            public float alpha { get; set; }
            public bool alphaTestEnable { get; set; }
            public int blend { get; set; }
            public int curringType { get; set; }
            public int layerType { get; set; }
            public int mapChipBottom { get; set; }
            public int mapChipLeft { get; set; }
            public int mapChipRight { get; set; }
            public int mapChipTop { get; set; }
            public int maxChipHeight { get; set; }
            public int maxChipWidth { get; set; }
            public float scale { get; set; }
            public int textureFilter { get; set; }
            public bool zTestEnable { get; set; }
            public bool zWriteEnable { get; set; }
        }

        public class Element : ISerializable
        {
            public int resourceID { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public float scale_x { get; set; }
            public float scale_y { get; set; }
            public float rotate { get; set; }
            public int aabb_x { get; set; }
            public int aabb_y { get; set; }
            public int aabb_w { get; set; }
            public int aabb_h { get; set; }

            public void Read(BinaryInputStream s)
            {
                resourceID = s.ReadInt32();
                x = s.ReadInt32();
                y = s.ReadInt32();
                scale_x = s.ReadFloat();
                scale_y = s.ReadFloat();
                rotate = s.ReadFloat();
                aabb_x = s.ReadInt32();
                aabb_y = s.ReadInt32();
                aabb_w = s.ReadInt32();
                aabb_h = s.ReadInt32();
            }

            public void Write(BinaryOutputStream s)
            {
                s.WriteInt32(resourceID);
                s.WriteInt32(x);
                s.WriteInt32(y);
                s.WriteFloat(scale_x);
                s.WriteFloat(scale_y);
                s.WriteFloat(rotate);
                s.WriteInt32(aabb_x);
                s.WriteInt32(aabb_y);
                s.WriteInt32(aabb_w);
                s.WriteInt32(aabb_h);
            }

            public static Element CreateDefault(int res)
            {
                Element ret = new Element();
                ret.resourceID = res;
                ret.scale_x = ret.scale_y = 1.0f;
                return ret;
            }

            public bool CalculateAABB(ActObject file)
            {
                var chip = file.mcd.FindChip(resourceID);
                if (chip == null) return false;
                var res = file.mcd.FindRes(chip.resourceID);
                if (res == null) return false;
                var bitmap = file.CreateBitmapForResource(resourceID);
                float w = bitmap.Width, h = bitmap.Height;

                //TODO
                if (Math.Abs(scale_x) != Math.Abs(scale_y)) return false;

                double ww = w * Math.Abs(scale_x) / 2.0f;
                double hh = h * Math.Abs(scale_y) / 2.0f;

                double r = rotate / 180 * 3.1416;
                ww = Math.Abs(ww * Math.Cos(r)) + Math.Abs(hh * Math.Sin(r));
                hh = Math.Abs(hh * Math.Cos(r)) + Math.Abs(ww * Math.Sin(r));

                aabb_w = (int)Math.Round(ww * 2);
                aabb_h = (int)Math.Round(hh * 2);
                aabb_x = x + (int)Math.Round(w / 2.0 - ww);
                aabb_y = y + (int)Math.Round(h / 2.0 - hh);

                return true;
            }
        }

        public Properties properties = new Properties();
        public bool is_short;
        public List<Element> elements;

        public override void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);

            int size = s.ReadInt32();
            int len = s.ReadInt32();
            switch (len)
            {
                case 24: is_short = true; break;
                case 40: is_short = false; break;
                default:
                    throw new Exception("invalid element size in layout");
            }

            elements = s.ReadSerializableArray(size, bs => new Element());
        }

        public override void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);

            s.WriteInt32(elements.Count);
            s.WriteInt32(is_short ? 24 : 40);

            s.WriteSerializableArray(elements, (bs, i) => { });
        }

        public static Act2DMapLayoutObject CreateDefault()
        {
            Act2DMapLayoutObject ret = new Act2DMapLayoutObject();
            ret.elements = new List<Element>();
            return ret;
        }

        public void CalculateAABB(ActObject file)
        {
            int l = 0, r = 0, t = 0, b = 0;
            int w = 0, h = 0;
            foreach (var element in elements)
            {
                if (element.CalculateAABB(file))
                {
                    if (element.aabb_x < l) l = element.aabb_x;
                    if (element.aabb_y < t) t = element.aabb_y;
                    if (element.aabb_x + element.aabb_w > r) r = element.aabb_x + element.aabb_w;
                    if (element.aabb_y + element.aabb_h > b) b = element.aabb_y + element.aabb_h;
                    if (element.aabb_w > w) w = element.aabb_w;
                    if (element.aabb_h > h) h = element.aabb_h;
                }
            }
            properties.mapChipLeft = l;
            properties.mapChipRight = r;
            properties.mapChipTop = t;
            properties.mapChipBottom = b;
            properties.maxChipWidth = w * 2;
            properties.maxChipHeight = h * 2;
        }
    }
}
