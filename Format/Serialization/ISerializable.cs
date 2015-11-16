using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.Serialization
{
    interface ISerializable
    {
        void Read(BinaryInputStream s);
        void Write(BinaryOutputStream s);
    }
}
