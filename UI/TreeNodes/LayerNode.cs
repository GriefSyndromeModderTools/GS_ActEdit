using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.UI.TreeNodes
{
    class LayerNode : AbstractNode
    {
        private FormActFile form;
        public ActLayerObject layer;
        private KeyNode key;
        private CodeNode code;

        public LayerNode(ActLayerObject layer, FormActFile form)
        {
            this.layer = layer;
            this.form = form;

            this.Text = layer.properties.stName;
            this.ImageIndex = 1;
            this.SelectedImageIndex = 1;
            this.ContextMenuStrip = form.ContextMenuLayer;

            ResetChildren();
        }

        private void ResetChildren()
        {
            this.Nodes.Clear();
            
            key = new KeyNode(layer.keys.First(), form);
            code = new CodeNode(layer.code, form);

            this.Nodes.Clear();
            this.Nodes.Add(key);
            this.Nodes.Add(code);
        }

        public override object GetPropertyObject()
        {
            return layer.properties;
        }

        public override void UpdateObject()
        {
            this.Text = layer.properties.stName;
            this.ResetChildren();
            this.Collapse();
        }
    }
}
