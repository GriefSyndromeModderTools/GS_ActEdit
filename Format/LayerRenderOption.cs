using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format
{
    class LayerRenderOption
    {
        [DisplayName("LayerName")]
        public string name { get; set; }
        [DisplayName("SpeedX")]
        public float ratio_x { get; set; }
        [DisplayName("SpeedY")]
        public float ratio_y { get; set; }
        [DisplayName("OffsetX")]
        public float offset_x { get; set; }
        [DisplayName("OffsetY")]
        public float offset_y { get; set; }

        public float CalculateX(float x)
        {
            return x * (1.0f - ratio_x) - offset_x;
        }

        public float CalculateY(float y)
        {
            return y * (1.0f - ratio_y) - offset_y;
        }

        public LayerRenderOption()
        {
            this.name = "???";
        }

        public LayerRenderOption(string name, float r_x)
        {
            this.name = name;
            this.ratio_x = r_x;
            this.ratio_y = 0.0f;
            this.offset_x = 0.0f;
            this.offset_y = 0.0f;
        }

        public byte[] Write()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(name);
                        bw.Write(ratio_x);
                        bw.Write(ratio_y);
                        bw.Write(offset_x);
                        bw.Write(offset_y);
                    }
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Read(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        name = br.ReadString();
                        ratio_x = br.ReadSingle();
                        ratio_y = br.ReadSingle();
                        offset_x = br.ReadSingle();
                        offset_y = br.ReadSingle();
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
