using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit
{
    public partial class FormShowImage : Form
    {
        public Image img;

        public FormShowImage(Image img)
        {
            InitializeComponent();
            this.img = img;
        }

        private void FormShowImage_Load(object sender, EventArgs e)
        {
            if (img.Tag != null && img.Tag is string)
            {
                this.Text = (string)img.Tag;
            }
            this.ClientSize = img.Size;
            this.BackgroundImage = img;
        }

    }
}
