using System.Collections.Generic;
using Kean.Core.Uri;
using Kean.Math.Geometry2D.Single;

namespace Hihapanesu.Interfaces
{
    public interface IGenerator
    {
        Size Feed { get; set; }
        bool Help { get; set; }
        Size Offset { get; set; }
        Size PageSize { get; set; }

        //void Append(char consonant, char vowel);
        void Append(IEnumerable<char> input);
        //void Append(IEnumerator<char> input);
        void AppendWhitespace();
        bool Save(Locator resource);
    }
}