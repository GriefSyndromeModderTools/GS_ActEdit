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

        public ListViewItemElement(Act2DMapLayoutObject.Element element, ActObject file)
        {
            this.element = element;

            this.Text = file.GetNameForResourceID(element.resourceID);
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
