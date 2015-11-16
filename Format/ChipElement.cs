using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    class ChipElement : ISerializable
    {
        public int chipID;
        public int resourceID;
        public short src_x, src_y;
        public short width, height;

        public short collision_flag;
        public short collision_x, collision_y;
        public short collision_w, collision_h;

        public void Read(BinaryInputStream s)
        {
            chipID = s.ReadInt32();
            resourceID = s.ReadInt32();

            src_x = s.ReadInt16();
            src_y = s.ReadInt16();
            width = s.ReadInt16();
            height = s.ReadInt16();

            s.ReadInt32(0);
            s.ReadInt32(0);
            s.ReadInt32(0);
            s.ReadInt32(0);
            s.ReadInt16(0);

            collision_flag = s.ReadInt16();
            collision_x = s.ReadInt16();
            collision_y = s.ReadInt16();
            collision_w = s.ReadInt16();
            collision_h = s.ReadInt16();

            s.ReadInt32(0);
            s.ReadInt32(0);

            s.ReadInt32(0);
            s.ReadInt32();
        }

        public void Write(BinaryOutputStream s)
        {
            s.WriteInt32(chipID);
            s.WriteInt32(resourceID);

            s.WriteInt16(src_x);
            s.WriteInt16(src_y);
            s.WriteInt16(width);
            s.WriteInt16(height);

            s.WriteInt32(0);
            s.WriteInt32(0);
            s.WriteInt32(0);
            s.WriteInt32(0);
            s.WriteInt16(0);

            s.WriteInt16(collision_flag);
            s.WriteInt16(collision_x);
            s.WriteInt16(collision_y);
            s.WriteInt16(collision_w);
            s.WriteInt16(collision_h);

            s.WriteInt32(0);
            s.WriteInt32(0);
        }
    }
}
