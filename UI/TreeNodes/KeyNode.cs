using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI.TreeNodes
{
    class KeyNode : AbstractNode
    {
        private ActKeyObject key;
        private FormActFile form;
        private MapLayoutNode layout;

        public KeyNode(ActKeyObject key, FormActFile form)
        {
            this.key = key;
            this.form = form;

            this.Text = "Key";
            this.ImageIndex = 2;
            this.SelectedImageIndex = 2;
            this.ContextMenuStrip = null;

            this.ResetChildren();
        }

        private void ResetChildren()
        {
            if (key.layout == null || !(key.layout is Act2DMapLayoutObject))
            {
                MessageBox.Show("the key must contain one layout", "Error");
                return;
            }
            layout = new MapLayoutNode((Act2DMapLayoutObject) key.layout, form);

            this.Nodes.Clear();
            this.Nodes.Add(layout);
        }

        public override void UpdateObject()
        {
            this.ResetChildren();
            this.ExpandThis();
        }

        public override object GetPropertyObject()
        {
            return key.properties;
        }
    }
}
