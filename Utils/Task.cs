using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_ActEdit.Utils
{
    class Task
    {
        public static void SendError(string str)
        {
            throw new Exception(str);
        }
    }
}
