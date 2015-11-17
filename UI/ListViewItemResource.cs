using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI
{
    class ListViewItemResource : ListViewItem
    {
        public int id;

        public ListViewItemResource(int id, ActObject file)
        {
            this.id = id;

            this.Text = id.ToString();
            this.ImageIndex = 5;

            var sub = new ListViewSubItem();
            sub.Name = "TextureName";
            sub.Text = file.GetNameForResourceID(id);
            this.SubItems.Add(sub);
        }
    }
}
