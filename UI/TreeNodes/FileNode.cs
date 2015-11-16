using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI.TreeNodes
{
    class FileNode : AbstractNode
    {
        private ActObject file;
        private FormActFile form;
        private TreeNode group_layer;
        private TreeNode code_node;
        private TreeNode group_res;

        //information used by resource acces

        private ActResourceChipObject chip;

        public FileNode(ActObject file, FormActFile form)
        {
            this.file = file;
            this.form = form;

            this.Text = "File";
            this.ImageIndex = 0;
            this.SelectedImageIndex = 0;
            this.ContextMenuStrip = form.ContextMenuFile;

            this.ResetChildren();

            this.ExpandThis();
        }

        private void ResetChildren()
        {
            group_layer = new TreeNode("Layers", 1, 1);
            group_layer.ContextMenuStrip = form.ContextMenuFileLayers;

            code_node = new CodeNode(file.code, form);

            group_res = new TreeNode("Resources", 5, 5);
            group_res.ContextMenuStrip = form.ContextMenuFileResources;

            Nodes.Clear();
            Nodes.Add(group_layer);
            Nodes.Add(code_node);
            Nodes.Add(group_res);

            foreach (var layer_obj in file.layers)
            {
                group_layer.Nodes.Add(new LayerNode(layer_obj, form));
            }

            if (file.resources.Count == 1 && file.resources.First() is ActResourceChipObject)
            {
                this.chip = (ActResourceChipObject) file.resources.First();
            }
            else
            {
                MessageBox.Show("unsupported resource list");
            }
        }

        public override object GetPropertyObject()
        {
            return file.properties;
        }

        public override void UpdateObject()
        {
            this.ResetChildren();
            this.ExpandThis();
            form.SetNodeSelected(this);
        }

        public void AddLayer()
        {
            ActLayerObject obj = ActLayerObject.CreateDefault("new_layer", GetNextLayerID(), GetDefaultResourceID());
            file.layers.Add(obj);
            LayerNode node = new LayerNode(obj, form);
            group_layer.Nodes.Add(node);
            form.SetNodeSelected(node);
        }

        private int GetDefaultResourceID()
        {
            return chip.properties.resourceID;
        }

        private int GetNextLayerID()
        {
            int next = 1;
            foreach (var l in file.layers)
            {
                if (l.properties.layerID >= next)
                {
                    next = l.properties.layerID + 1;
                }
            }
            return next;
        }

        public string GetNameForResourceID(int id)
        {
            if (file.mcd == null)
            {
                return "???";
            }
            ChipElement chip = file.mcd.FindChip(id);
            if (chip == null) return "???";
            AbstractResourceInfoObject res = file.mcd.FindRes(chip.resourceID);
            if (res == null) return "???";
            return res.GetDisplayName();
        }
    }
}
