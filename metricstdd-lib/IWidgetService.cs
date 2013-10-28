using metricstdd_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metricstdd
{
    public interface IWidgetService
    {
        int GetNumber();

        CachedNumber GetNumberFromCache();

        void UpdateNumberInCache(CachedNumber cachedNumber);
    }
}
