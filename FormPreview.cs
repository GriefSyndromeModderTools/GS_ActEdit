using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GS_ActEdit
{
    partial class FormPreview : Form
    {
        private Act2DMapLayoutObject layout;
        private ActObject file;
        private float scale = 0.5f;

        public FormPreview(Act2DMapLayoutObject layout, ActObject file)
        {
            InitializeComponent();

            this.layout = layout;
            this.file = file;
        }

        private Point GetCenterOnMap()
        {
            Size p_ctrl = new Size(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            p_ctrl -= offset;
            return new Point((int)(p_ctrl.Width / scale), (int)(p_ctrl.Height / scale));
        }

        private void SetScale(float val)
        {
            Point center = GetCenterOnMap();
            center.X = (int)(center.X * val);
            center.Y = (int)(center.Y * val);
            center.X = pictureBox1.ClientSize.Width / 2 - center.X;
            center.Y = pictureBox1.ClientSize.Height / 2 - center.Y;
            offset = new Size(center.X, center.Y);
            scale = val;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            Matrix te = new Matrix();
            te.Translate(offset.Width, offset.Height);
            te.Scale(scale, scale);
            //int count = 100;
            foreach (var element in layout.elements)
            {
                g.Transform = te;
                DrawElement(g, element);
                //if (--count == 0) break;
            }
        }

        private void DrawElement(Graphics g, Act2DMapLayoutObject.Element e)
        {
            var bitmap = file.CreateBitmapForResource(e.resourceID);
            if (bitmap == null) return;

            g.TranslateTransform(e.x, e.y);
            g.TranslateTransform(bitmap.Width * 0.5f, bitmap.Height * 0.5f);
            g.RotateTransform(e.rotate);
            g.ScaleTransform(e.scale_x, e.scale_y);
            g.TranslateTransform(-bitmap.Width * 0.5f, -bitmap.Height * 0.5f);
            g.DrawImage(bitmap, new PointF(0.0f, 0.0f));
        }

        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {
            //scale *= 1.2f;
            SetScale(scale * 1.2f);
            pictureBox1.Refresh();
        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {
            //scale /= 1.2f;
            SetScale(scale / 1.2f);
            pictureBox1.Refresh();
        }

        private Size offset;
        private Size down_offset;
        private Size p_down;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                p_down = new Size(e.Location);
                down_offset = offset;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                offset = down_offset + new Size(e.Location) - p_down;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                offset = down_offset + new Size(e.Location) - p_down;
                pictureBox1.Refresh();
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}
