using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hihapanesu.Interfaces
{
    public interface ITranscriber
    {
        string Transcribe(string input);
        System.Collections.Generic.IEnumerable<char> Transcribe(System.Collections.Generic.IEnumerable<char> input);
    }
}
