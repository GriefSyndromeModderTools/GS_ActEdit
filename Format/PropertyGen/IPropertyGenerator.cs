using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.PropertyGen
{
    interface IPropertyGenerator
    {
        void Generate(PropertyInfo parent_property, TypeBuilder child_builder, FieldInfo parent_field);
    }
}
