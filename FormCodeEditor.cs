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

namespace GS_ActEdit
{
    public partial class FormCodeEditor : Form
    {
        private CodeObject code;
        private string last_str;
        private bool is_in_activating = false;

        public FormCodeEditor(object code)
        {
            InitializeComponent();
            this.code = (CodeObject) code;
        }

        private void LoadStr()
        {
            last_str = code.code;
            textBox1.Text = last_str;
        }

        private void FormCodeEditor_Load(object sender, EventArgs e)
        {
            LoadStr();
        }

        private void FormCodeEditor_Activated(object sender, EventArgs e)
        {
            if (is_in_activating) return;
            is_in_activating = true;
            if (last_str != code.code)
            {
                if (MessageBox.Show("Code has changed. Reload?", "Code Editor", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    LoadStr();
                }
                else
                {
                    last_str = code.code;
                }
            }
            is_in_activating = false;
        }

        private void FormCodeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (last_str != textBox1.Text)
            {
                if (MessageBox.Show("Save changes?", "Code Editor", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    code.code = textBox1.Text;
                }
            }
        }
    }
}
