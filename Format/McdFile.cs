using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    static class McdFile
    {
        public static McdObject ReadMcdFromFile(string filename)
        {
            try
            {
                using (FileStream f = File.Open(filename, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(f))
                    {
                        BinaryInputStream bs = new BinaryInputStream(br);
                        bs.ReadInt32(0x434D4432);
                        bs.ReadInt32(2);
                        bs.ReadInt32(0);
                        McdObject mcd = new McdObject();
                        mcd.Read(bs);
                        return mcd;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
