using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    class McdObject : ISerializable
    {
        public List<ChipElement> chips = new List<ChipElement>();
        public List<AbstractResourceInfoObject> resources = new List<AbstractResourceInfoObject>(); 

        public void Read(BinaryInputStream s)
        {
            int size = s.ReadInt32();
            int len = s.ReadInt32();
            if (len != 56)
            {
                throw new Exception("invalid element size in layout");
            }
            chips = s.ReadSerializableArray(size, bs => new ChipElement());

            resources = s.ReadObjectArray<AbstractResourceInfoObject>();
        }

        public void Write(BinaryOutputStream s)
        {
            throw new NotImplementedException();
        }

        public ChipElement FindChip(int id)
        {
            foreach (var chip in chips)
            {
                if (chip.chipID == id) return chip;
            }
            return null;
        }

        public AbstractResourceInfoObject FindRes(int id)
        {
            foreach (var res in resources)
            {
                if (res.GetId() == id)
                {
                    return res;
                }
            }
            return null;
        }
    }
}
