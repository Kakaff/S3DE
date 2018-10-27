using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Utility
{
    internal static class InstanceCreator
    {
        internal static T CreateInstance<T>() where T : class
        {
            T instance;
            instance = (T)Activator.CreateInstance(typeof(T));
            return instance;
        }
    }
}
