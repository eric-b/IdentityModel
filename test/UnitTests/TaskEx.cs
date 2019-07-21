using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModel.UnitTests
{
    internal static class TaskEx
    {
        public static Task CompletedTask
        {
            get
            {
#if NET45
                return Task.FromResult(0);
#else
                return Task.CompletedTask;
#endif
            }
        }

    }
}
