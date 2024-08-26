using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace GoldNet.JXKP
{
    internal class AjaxAction
    {
        public AjaxAction(MethodInfo method)
        {
            this.Method = method;
            this.Parameters = this.Method.GetParameters();
        }

        public MethodInfo Method { get; private set; }

        public ParameterInfo[] Parameters { get; private set; }
    }
}
