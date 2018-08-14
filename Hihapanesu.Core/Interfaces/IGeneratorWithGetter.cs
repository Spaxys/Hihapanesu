using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hihapanesu.Core.Interfaces
{
    public interface IGeneratorWithGetter
    {
        Kean.Xml.Dom.Document GetPage();
    }
}
