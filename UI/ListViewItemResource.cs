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
        public Form form;

        public ListViewItemResource(int id, FormActFile form)
        {
            this.id = id;
            this.form = form;

            this.Text = id.ToString();
            this.ImageIndex = 5;

            var sub = new ListViewSubItem();
            sub.Name = "TextureName";
            sub.Text = form.root.GetNameForResourceID(id);
            this.SubItems.Add(sub);
        }
    }
}
