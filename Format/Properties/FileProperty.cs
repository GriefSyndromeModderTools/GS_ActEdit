using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS_ActEdit.Format.PropertyGen;
using System.ComponentModel;

namespace GS_ActEdit.Format.Properties
{
    public class FileProperty
    {
        [DisplayName("Canvas B")]
        [Description("Not confirmed. Blue value of canvas color.")]
        [Category("Scene")]
        [PropertyAttributes.SimpleLock]
        [PropertyAttributes.SimpleLockConst(256)]
        public int canvasB { get; set; }

        public object GetSimpleModeObject()
        {
            return PropertyObjectGenerator.Build(this, "simple");
        }

        //private bool ShouldSerializecanvasB() { return canvasB != 255; }

        public FileProperty()
        {
            simple_props = PropertyObjectGenerator.Build(this, "simple");
            adv_props = PropertyObjectGenerator.Build(this, "advanced");
        }

        private object simple_props;
        private object adv_props;
    }
}
