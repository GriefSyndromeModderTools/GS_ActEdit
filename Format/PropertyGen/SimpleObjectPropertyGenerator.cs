using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.PropertyGen
{
    class SimpleObjectPropertyGenerator : IPropertyGenerator
    {
        private CustomAttributeBuilder CreateAttributeBuilder<TAttr, TVal>(TVal val)
        {
            return new CustomAttributeBuilder(
                typeof(TAttr).GetConstructor(new Type[] { typeof(TVal) }),
                new object[] { val });
        }

        private void CopyAttributes(PropertyBuilder pb, PropertyInfo pi)
        {
            {
                var attr = pi.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                {
                    pb.SetCustomAttribute(CreateAttributeBuilder<DisplayNameAttribute, string>(attr.DisplayName));
                }
            }
            {
                var attr = pi.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null)
                {
                    pb.SetCustomAttribute(CreateAttributeBuilder<DescriptionAttribute, string>(attr.Description));
                }
            }
            {
                var attr = pi.GetCustomAttribute<CategoryAttribute>();
                if (attr != null)
                {
                    pb.SetCustomAttribute(CreateAttributeBuilder<CategoryAttribute, string>(attr.Category));
                }
            }
        }

        public void Generate(PropertyInfo parent_property, TypeBuilder child_builder, FieldInfo parent_field)
        {
            var attr = parent_property.GetCustomAttribute<PropertyAttributes.SimpleLockAttribute>();
            if (attr != null)
            {
                return;
            }

            TypeInfo parent = parent_property.DeclaringType.GetTypeInfo();

            var f_p = child_builder.DefineField("_" + parent_property.Name, 
                parent_property.PropertyType, 
                FieldAttributes.Public);

            var pb = child_builder.DefineProperty(parent_property.Name, 
                System.Reflection.PropertyAttributes.None,
                parent_property.PropertyType, null);

            CopyAttributes(pb, parent_property);

            var set_m = child_builder.DefineMethod("set_" + parent_property.Name,
                MethodAttributes.Public, 
                null, new[] { parent_property.PropertyType });
            var get_m = child_builder.DefineMethod("get_" + parent_property.Name,
                MethodAttributes.Public, 
                parent_property.PropertyType, Type.EmptyTypes);

            var set_il = set_m.GetILGenerator();
            set_il.Emit(OpCodes.Ldarg_0);
            set_il.Emit(OpCodes.Ldfld, parent_field);
            set_il.Emit(OpCodes.Castclass, parent);
            set_il.Emit(OpCodes.Ldarg_1);
            set_il.EmitCall(OpCodes.Call, parent.GetMethod("set_" + parent_property.Name), null);
            set_il.Emit(OpCodes.Ret);

            var get_il = get_m.GetILGenerator();
            get_il.Emit(OpCodes.Ldarg_0);
            get_il.Emit(OpCodes.Ldfld, parent_field);
            get_il.Emit(OpCodes.Castclass, parent);
            get_il.EmitCall(OpCodes.Call, parent.GetMethod("get_" + parent_property.Name), null);
            get_il.Emit(OpCodes.Ret);

            pb.SetGetMethod(get_m);
            pb.SetSetMethod(set_m);
        }
    }
}
