using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI
{
    class ListViewItemElement : ListViewItem
    {
        private Act2DMapLayoutObject.Element element;
        private FormActFile form;

        public ListViewItemElement(Act2DMapLayoutObject.Element element, FormActFile form)
        {
            this.element = element;
            this.form = form;

            this.Text = form.root.GetNameForResourceID(element.resourceID);
            this.ImageIndex = 0;
        }

        public object GetPropertyObject()
        {
            return element;
        }

        public Act2DMapLayoutObject.Element GetElement()
        {
            return element;
        }
    }
}
