using System;

namespace Bubbio.Core.Helpers
{
    public static class GuidEx
    {
        public static bool IsEmpty(this Guid guid)
        {
            return guid.Equals(Guid.Empty);
        }
    }
}