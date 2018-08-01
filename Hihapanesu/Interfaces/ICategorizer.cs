using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hihapanesu.Interfaces
{
    public interface ICategorizer
    {
        LetterCategory Categorize(char c);
    }
}
