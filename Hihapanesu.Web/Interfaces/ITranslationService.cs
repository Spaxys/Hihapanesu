using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hihapanesu.Web.Interfaces
{
    public interface ITranslationService
    {
        string Transcribe(string input);
        Kean.Xml.Dom.Document Generate(string input);
        Kean.Xml.Dom.Document GenerateWithTest(string input);
        Kean.Xml.Dom.Document TranscribeAndGenerate(string input);
        Kean.Xml.Dom.Document TranscribeAndGenerate(string input, bool test);
    }
}