using GS_ActEdit.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_ActEdit.UI.TreeNodes
{
    class CodeNode : AbstractNode
    {
        private CodeObject code;

        public CodeNode(CodeObject code, FormActFile form)
        {
            this.code = code;

            this.Text = "Code";
            this.ImageIndex = 6;
            this.SelectedImageIndex = 6;
            this.ContextMenuStrip = form.ContextMenuCode;
        }

        public override void UpdateObject()
        {
            this.ExpandThis();
        }

        public override object GetPropertyObject()
        {
            return code.properties;
        }

        public void EditCode()
        {
            new FormCodeEditor(code).Show(null);
        }
    }
}
