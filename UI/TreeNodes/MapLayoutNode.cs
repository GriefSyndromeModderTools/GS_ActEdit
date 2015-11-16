using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.UI.TreeNodes
{
    class MapLayoutNode : AbstractNode
    {
        private Act2DMapLayoutObject layout;
        private FormActFile form;

        public MapLayoutNode(Act2DMapLayoutObject layout, FormActFile form)
        {
            this.layout = layout;
            this.form = form;

            this.Text = "Layout";
            this.ImageIndex = 4;
            this.SelectedImageIndex = 4;
        }

        public override void UpdateObject()
        {
        }

        public override object GetPropertyObject()
        {
            return layout.properties;
        }

        public Act2DMapLayoutObject GetLayout()
        {
            return layout;
        }
    }
}
