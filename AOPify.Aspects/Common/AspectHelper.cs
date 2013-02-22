using System.Runtime.Remoting.Messaging;
using System.Text;

namespace AOPify.Aspects.Common
{
    public static class AspectHelper
    {
        public static string GetFormattedInputParameters(IMethodMessage methodMessageCall)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("(");

            for (int i = 0; i < methodMessageCall.ArgCount; i++)
            {
                stringBuilder.Append("{0}:{1},".FormatWith(methodMessageCall.GetArgName(i), methodMessageCall.Args[i]));
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }    
    }
}