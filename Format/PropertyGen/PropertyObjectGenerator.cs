using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.PropertyGen
{
    class PropertyObjectGenerator
    {
        private static ModuleBuilder mb;
        private static Dictionary<string, IPropertyGenerator> pg = new Dictionary<string, IPropertyGenerator>()
        {
            { "simple", new SimpleObjectPropertyGenerator() },
        };

        private TypeInfo parent;
        private string suffix;

        private TypeBuilder builder;
        private FieldInfo f_parent;

        private PropertyObjectGenerator(TypeInfo parent, string suffix)
        {
            this.parent = parent;
            this.suffix = suffix;
        }

        private TypeInfo GenerateType()
        {
            CreateBuilder();
            BuildConstructor();
            BuildProperties();
            return builder.CreateTypeInfo();
        }

        private void BuildConstructor()
        {
            f_parent = builder.DefineField("parent", typeof(object), FieldAttributes.Private);
            var cb = builder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[] { typeof(object) });
            var il = cb.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, f_parent);
            il.Emit(OpCodes.Ret);
        }

        private void BuildProperty(PropertyInfo p)
        {
            if (pg.ContainsKey(suffix))
            {
                pg[suffix].Generate(p, builder, f_parent);
            }
        }

        private void BuildProperties()
        {
            foreach (var p in parent.GetProperties())
            {
                BuildProperty(p);
            }
        }

        private ModuleBuilder CreateModuleBuilder()
        {
            if (mb == null)
            {
                var an = new AssemblyName("ActEdit_Dynamic");
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an,
                    AssemblyBuilderAccess.Run);
                mb = assemblyBuilder.DefineDynamicModule("MainModule");
            }
            return mb;
        }

        private void CreateBuilder()
        {
            string type_name = parent.FullName + "_dynamic_" + suffix;
            builder = CreateModuleBuilder().DefineType(type_name,
                                TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout,
                                null);
        }

        private static Dictionary<TypeInfo, Dictionary<string, TypeInfo>> cached_types = new Dictionary<TypeInfo,Dictionary<string,TypeInfo>>();

        public static object Build(object parent, string suffix)
        {
            TypeInfo ti_parent = parent.GetType().GetTypeInfo();
            if (!cached_types.ContainsKey(ti_parent))
            {
                cached_types.Add(ti_parent, new Dictionary<string, TypeInfo>());
            }
            if (!cached_types[ti_parent].ContainsKey(suffix))
            {
                TypeInfo ti_child = new PropertyObjectGenerator(ti_parent, suffix).GenerateType();
                cached_types[ti_parent].Add(suffix, ti_child);
            }
            return cached_types[ti_parent][suffix].GetConstructor(new Type[] { typeof(object) }).Invoke(new object[] { parent });
        }
    }
}
