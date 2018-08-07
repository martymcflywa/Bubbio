using System;

namespace Bubbio.Core.Helpers
{
    public static class TypeEx
    {
        public static bool IsATypeOf(this Type subType, Type baseType)
        {
            return subType.IsSubclassOf(baseType) || subType == baseType;
        }
    }
}