using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit
{
    partial class FormMultiPreview : Form
    {
        private ActObject file;
        private List<LayerRenderOption> layers;

        public FormMultiPreview(ActObject file, List<LayerRenderOption> layers)
        {
            InitializeComponent();
            this.file = file;
            this.layers = layers;

            var start_layer = file.FindLayerByName("start");
            if (start_layer != null)
            {
                this.camera_x = start_layer.GetLayout().elements[0].x;
                this.camera_y = start_layer.GetLayout().elements[0].y - 180;
            }
            else
            {
                this.camera_x = 400;
                this.camera_y = 428;
            }

            if (this.camera_x < file.properties.marginLeft + 400)
            {
                this.camera_x = file.properties.marginLeft + 400;
            }
            if (this.camera_x > file.properties.screenWidth - file.properties.marginRight - 400)
            {
                this.camera_x = file.properties.screenWidth - file.properties.marginRight - 400;
            }
            if (this.camera_y < file.properties.marginTop + 300)
            {
                this.camera_y = file.properties.marginTop + 300;
            }
            if (this.camera_y > file.properties.screenHeight - file.properties.marginBottom - 300)
            {
                this.camera_y = file.properties.screenHeight - file.properties.marginBottom - 300;
            }
        }

        private void DrawElement(Graphics g, Act2DMapLayoutObject.Element e)
        {
            var bitmap = file.CreateBitmapForResource(e.resourceID);
            if (bitmap == null) return;

            var saved = g.Save();
            g.TranslateTransform(e.x, e.y);
            g.TranslateTransform(bitmap.Width * 0.5f, bitmap.Height * 0.5f);
            g.RotateTransform(e.rotate);
            g.ScaleTransform(e.scale_x, e.scale_y);
            g.TranslateTransform(-bitmap.Width * 0.5f, -bitmap.Height * 0.5f);
            g.DrawImage(bitmap, new PointF(0.0f, 0.0f));
            g.Restore(saved);
        }

        private void RenderLayer(Graphics g, float x, float y, ActLayerObject layer)
        {
            var saved = g.Save();
            g.TranslateTransform(-x, -y);
            foreach (var element in layer.GetLayout().elements)
            {
                DrawElement(g, element);
            }
            g.Restore(saved);
        }

        private void RenderLayerWithCameraRatio(Graphics g, float x, float y, LayerRenderOption layer)
        {
            RenderLayer(g, layer.CalculateX(x) - 400.0f, layer.CalculateY(y) - 300.0f, file.FindLayerByName(layer.name));
        }

        private void RenderLayerWithCameraRatio(Graphics g, float x, float y, float ratio, ActLayerObject l)
        {
            RenderLayer(g, (x * (1.0f - ratio)) - 400, y - 300, l);
        }

        private void RenderAll(Graphics g, float x, float y)
        {
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;

            foreach (var l in layers)
            {
                RenderLayerWithCameraRatio(g, x, y, l);
            }
        }

        float camera_x, camera_y;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            float screen_x = pictureBox1.ClientSize.Width / 2.0f;
            float screen_y = pictureBox1.ClientSize.Height / 2.0f;
            float scale;
            if (screen_x * 600 > screen_y * 800)
            {
                scale = screen_y / 300;
                e.Graphics.TranslateTransform(screen_x - scale * 400.0f, 0.0f);
            }
            else
            {
                scale = screen_x / 400;
                e.Graphics.TranslateTransform(0.0f, screen_y - scale * 300.0f);
            }
            e.Graphics.ScaleTransform(scale, scale);
            e.Graphics.SetClip(new Rectangle(0, 0, 800, 600));
            RenderAll(e.Graphics, camera_x, camera_y);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (no_update) return;
            pictureBox1.Refresh();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (no_update) return;
            pictureBox1.Refresh();
        }

        bool[] key_status = new bool[4];
        bool no_update = false;

        private void FormMultiPreview_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    key_status[0] = true; break;
                case Keys.Down:
                    key_status[1] = true; break;
                case Keys.Left:
                    key_status[2] = true; break;
                case Keys.Right:
                    key_status[3] = true; break;
            }
        }

        private void FormMultiPreview_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    key_status[0] = false; break;
                case Keys.Down:
                    key_status[1] = false; break;
                case Keys.Left:
                    key_status[2] = false; break;
                case Keys.Right:
                    key_status[3] = false; break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            no_update = true;
            if (key_status[0])
                camera_y -= 10;
            if (key_status[1])
                camera_y += 10;
            if (key_status[2])
                camera_x -= 10;
            if (key_status[3])
                camera_x += 10;
            no_update = false;
            pictureBox1.Refresh();
        }
    }
}
