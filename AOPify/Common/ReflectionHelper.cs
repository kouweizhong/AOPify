using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace AOPify.Common
{
    public static class ReflectionHelper
    {
        public static string GetMethodParamersWithNameValues(MethodBase method, params object[] values)
        {
            ParameterInfo[] parms = method.GetParameters();

            if (parms.Length != values.Length)
            {
                return "Parameter and name count is different!";
            }

            object[] namevalues = new object[2 * parms.Length];
            StringBuilder stringBuilder = new StringBuilder();

            for (int counter = 0, nameCounter = 0; counter < parms.Length; counter++, nameCounter += 2)
            {
                stringBuilder.Append("{" + nameCounter + "}={" + (nameCounter + 1) + "}, ");
                namevalues[nameCounter] = parms[counter].Name;
                if (counter < values.Length) namevalues[nameCounter + 1] = values[counter];
            }

            return stringBuilder.ToString().FormatWith(namevalues);
        }

        public static T[] GetAttributes<T>(IMethodCallMessage msg, bool inherit)
        {
            return msg.MethodBase.GetCustomAttributes(typeof(T), true) as T[];
        }
    }
}