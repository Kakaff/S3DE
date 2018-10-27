using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Components
{
    public interface IUpdateLogic
    {
        void EarlyUpdate();
        void Update();
        void LateUpdate();
    }
}
