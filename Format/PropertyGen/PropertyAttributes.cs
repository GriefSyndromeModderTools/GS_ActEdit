using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.PropertyGen
{
    static class PropertyAttributes
    {
        public class SimpleLockAttribute : Attribute
        {
        }

        public class SimpleLockConst : Attribute
        {
            private object val;

            public SimpleLockConst(object val)
            {
                this.val = val;
            }

            public object Value
            {
                get
                {
                    return val;
                }
            }
        }
    }
}
