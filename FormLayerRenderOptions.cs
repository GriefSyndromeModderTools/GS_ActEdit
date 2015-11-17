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
    partial class FormLayerRenderOptions : Form
    {
        private ActObject file;

        public FormLayerRenderOptions(ActObject file)
        {
            InitializeComponent();

            this.file = file;
        }

        private bool HasSelected()
        {
            return listView1.SelectedItems.Count > 0;
        }

        private int SelectedIndex()
        {
            return listView1.SelectedItems.OfType<ListViewItem>().First().Index;
        }

        private bool HasDataInClipboard()
        {
            return Clipboard.ContainsData("ActEdit_LayerRenderOption");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.renderLayers.Add(new LayerRenderOption("layer_", 0.0f));
            listView1.Items.Add(new ListViewItem("layer_"));
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HasSelected())
            {
                int id = SelectedIndex();
                LayerRenderOption l = file.renderLayers[id];
                var data = l.Write();
                if (data != null)
                {
                    file.renderLayers.RemoveAt(id);
                    listView1.Items.RemoveAt(id);
                    Clipboard.SetData("ActEdit_LayerRenderOption", data);
                }
            }
        }

        private void pasteBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HasDataInClipboard() && HasSelected())
            {
                int id = SelectedIndex();
                LayerRenderOption l = new LayerRenderOption();
                if (l.Read((byte[])Clipboard.GetData("ActEdit_LayerRenderOption")))
                {
                    file.renderLayers.Insert(id, l);
                    listView1.Items.Insert(id, new ListViewItem(l.name));
                }
            }
        }

        private void pasteAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HasDataInClipboard() && HasSelected())
            {
                int id = SelectedIndex();
                LayerRenderOption l = new LayerRenderOption();
                if (l.Read((byte[])Clipboard.GetData("ActEdit_LayerRenderOption")))
                {
                    file.renderLayers.Insert(id + 1, l);
                    listView1.Items.Insert(id + 1, new ListViewItem(l.name));
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HasSelected())
            {
                int id = SelectedIndex();
                LayerRenderOption l = file.renderLayers[id];
                file.renderLayers.RemoveAt(id);
                listView1.Items.RemoveAt(id);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            cutToolStripMenuItem.Enabled = HasSelected();
            pasteBeforeToolStripMenuItem.Enabled = HasDataInClipboard() && HasSelected();
            pasteAfterToolStripMenuItem.Enabled = HasDataInClipboard() && HasSelected();
            deleteToolStripMenuItem.Enabled = HasSelected();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HasSelected())
            {
                propertyGrid1.SelectedObject = file.renderLayers[SelectedIndex()];
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        private void FormLayerRenderOptions_Load(object sender, EventArgs e)
        {
            foreach (var l in file.renderLayers)
            {
                listView1.Items.Add(new ListViewItem(l.name));
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (HasSelected())
            {
                int id = SelectedIndex();
                listView1.Items[id].Text = file.renderLayers[id].name;
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(this.listView1, e.Location);
            }
        }
    }
}
