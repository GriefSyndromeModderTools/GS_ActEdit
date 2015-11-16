using GS_ActEdit.Format.Serialization;
using KUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable 649

namespace GS_ActEdit.Format
{
    [ObjectSerialization.ID(".?AVCAct@@")]
    class ActObject : ISerializable
    {
        public class Properties : PropertyTable
        {
            public int canvasB { get; set; }
            public int canvasG { get; set; }
            public int canvasR { get; set; }
            public bool loop { get; set; }
            public int marginBottom { get; set; }
            public int marginLeft { get; set; }
            public int marginRight { get; set; }
            public int marginTop { get; set; }
            public float offsetX { get; set; }
            public float offsetY { get; set; }
            public int resolutionMs { get; set; }
            public int screenHeight { get; set; }
            public int screenWidth { get; set; }
            public string stName { get; set; }
            public int templateLayerID { get; set; }
            public bool visible { get; set; }
        }

        public Properties properties = new Properties();
        public CodeObject code = new CodeObject();
        public List<ActLayerObject> layers;
        public List<AbstractActResourceObject> resources;

        public McdObject mcd = null;
        private string root_folder;

        public void Read(BinaryInputStream s)
        {
            properties.ReadFromStream(s);
            code.Read(s);
            layers = s.ReadObjectArray<ActLayerObject>();
            resources = s.ReadObjectArray<AbstractActResourceObject>();
        }

        public void Write(BinaryOutputStream s)
        {
            properties.WriteToStream(s);
            code.Write(s);
            s.WriteObjectArray(layers);
            s.WriteObjectArray(resources);
        }

        public void LinkRootFolder(string path)
        {
            this.root_folder = path;

            if (resources.Count != 1 || !(resources[0] is ActResourceChipObject))
            {
                MessageBox.Show("unsupported resource type");
                return;
            }
            string mcd_rel_path = ((ActResourceChipObject)resources[0]).properties.stChipFile;
            string mcd_path = Path.Combine(path, mcd_rel_path);
            McdObject mcd = McdFile.ReadMcdFromFile(mcd_path);
            if (mcd == null)
            {
                MessageBox.Show("can not open mcd file");
                return;
            }
            this.mcd = mcd;
            this.bitmap_cache.Clear();
        }

        public ActLayerObject FindLayerByName(string name)
        {
            foreach (var l in layers)
            {
                if (l.properties.stName == name)
                {
                    return l;
                }
            }
            return null;
        }

        private Bitmap MakeBitmapRegion(Bitmap src, int x, int y, int w, int h)
        {
            if (x + w > src.Width || y + h > src.Height)
            {
                int nw = x + w;
                int nh = y + h;
                var tile = new Bitmap(nw, nh);
                var g = Graphics.FromImage(tile);
                for (int ix = 0; ix < nw; ix += src.Width)
                {
                    for (int iy = 0; iy < nh; iy += src.Height)
                    {
                        g.DrawImage(src, new Point(ix, iy));
                    }
                }
                src = tile;
            }
            return src.Clone(new Rectangle(x, y, w, h), src.PixelFormat);
        }


        private Dictionary<int, Bitmap> bitmap_cache = new Dictionary<int, Bitmap>();

        public Bitmap CreateBitmapForResource(int id)
        {
            if (bitmap_cache.ContainsKey(id)) return bitmap_cache[id];
            if (mcd == null) return null;

            //make new bitmap
            var chip = mcd.FindChip(id);
            if (chip == null) return null;
            var res = mcd.FindRes(chip.resourceID);
            if (res == null) return null;
            if (!(res is ActTextureResourceInfoObject))
            {
                //unsupported format
                return null;
            }
            string path_rel = ((ActTextureResourceInfoObject)res).properties.stFilePath;
            string path = Path.Combine(this.root_folder, path_rel);

            //create bitmap
            if (Path.GetExtension(path) == ".png")
            {
                try
                {
                    string ex_path = Path.GetDirectoryName(path);
                    string ex_file = Path.GetFileNameWithoutExtension(path);
                    var ret = CreateBitmapFromCv2File(Path.Combine(ex_path, ex_file + ".cv2"));
                    //ret = ret.Clone(new Rectangle(chip.src_x, chip.src_y, chip.width, chip.height), ret.PixelFormat);
                    ret = MakeBitmapRegion(ret, chip.src_x, chip.src_y, chip.width, chip.height);
                    ret.Tag = Path.GetFileName(path);
                    bitmap_cache.Add(id, ret);
                    return ret;
                }
                catch
                {
                    return null;
                }
            }
            else if (Path.GetExtension(path) == ".dds")
            {
                try
                {
                    DDSImage dds = new DDSImage(File.ReadAllBytes(path));
                    if (dds.images.Length != 1)
                    {
                        return null;
                    }
                    var ret = dds.images[0];
                    //if (chip.width > ret.Width) chip.width = (short) ret.Width;
                    //if (chip.height > ret.Height) chip.height = (short)ret.Height;
                    //ret = ret.Clone(new Rectangle(chip.src_x, chip.src_y, chip.width, chip.height), ret.PixelFormat);
                    ret = MakeBitmapRegion(ret, chip.src_x, chip.src_y, chip.width, chip.height);
                    ret.Tag = Path.GetFileName(path);
                    bitmap_cache.Add(id, ret);
                    return ret;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private Bitmap CreateBitmapFromCv2File(string path)
        {
            using (FileStream f_in = File.OpenRead(path))
            {
                byte[] header = new byte[1 + 4 + 4 + 4 + 4];
                f_in.Read(header, 0, header.Length);
                int width = BitConverter.ToInt32(header, 1);
                int height = BitConverter.ToInt32(header, 5);
                int stride = BitConverter.ToInt32(header, 9);
                if (header[0] == 8)
                {
                    //don't support palette
                    return null;
                }
                Bitmap bitmap = new Bitmap(width, height);
                for (int j = 0; j < height; ++j) for (int i = 0; i < stride; ++i)
                    {
                        Color c;
                        byte[] buf = new byte[4];
                        f_in.Read(buf, 0, 4);
                        c = Color.FromArgb(buf[3], buf[2], buf[1], buf[0]); //bgra8888
                        if (i < width)
                            bitmap.SetPixel(i, j, c);
                    }
                return bitmap;
            }
        }
    }
}
