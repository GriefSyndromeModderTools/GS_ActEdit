using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI.TreeNodes
{
    abstract class AbstractNode : TreeNode
    {
        public void ExpandThis()
        {
            Expand();
            foreach (TreeNode tn in Nodes)
            {
                tn.Collapse();
            }
        }

        public virtual object GetPropertyObject()
        {
            return null;
        }

        public virtual void UpdateObject()
        {
        }
    }
}
