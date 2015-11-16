using GS_ActEdit.Format.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    static class ActFile
    {
        public static ActObject ReadActFromFile(string filename)
        {
            try
            {
                using (FileStream f = File.Open(filename, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(f))
                    {
                        BinaryInputStream bs = new BinaryInputStream(br);
                        bs.ReadInt32(0x31544341);
                        bs.ReadInt32(1);
                        bs.ReadInt32(0);
                        ActObject act = new ActObject();
                        act.Read(bs);
                        return act;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    
        public static bool SaveActToFile(this ActObject obj, string filename)
        {
            try
            {
                using (FileStream f = File.Open(filename, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(f))
                    {
                        BinaryOutputStream bs = new BinaryOutputStream(bw);
                        bs.WriteInt32(0x31544341);
                        bs.WriteInt32(1);
                        bs.WriteInt32(0);
                        obj.Write(bs);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
