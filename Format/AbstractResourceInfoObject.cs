using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    abstract class AbstractResourceInfoObject : ISerializable
    {
        public abstract void Read(BinaryInputStream s);
        public abstract void Write(BinaryOutputStream s);

        public abstract int GetId();
        public abstract string GetDisplayName();
    }
}
