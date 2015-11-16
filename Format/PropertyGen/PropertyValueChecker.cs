using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Format.PropertyGen
{
    class PropertyValueChecker
    {
        private class Entry
        {

        }

        private class CheckResult
        {
            public void SetNextCheck(string name)
            {

            }
        }

        private static void Check(CheckResult result, object obj)
        {

        }

        public static bool CheckProperties(ActObject act)
        {
            CheckResult result = new CheckResult();

            result.SetNextCheck("Scene");
            Check(result, act.properties);

            result.SetNextCheck("Scene.Code");
            Check(result, act.code.properties);

            //...
            return true;
        }
    }
}
