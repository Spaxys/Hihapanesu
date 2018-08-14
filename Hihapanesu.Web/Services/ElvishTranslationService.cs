using Hihapanesu.Generators;
using Hihapanesu.Interfaces;
using Hihapanesu.Transcribers;
using Hihapanesu.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hihapanesu.Web.Services
{
    public class ElvishTranslationService : ITranslationService
    {
        ITranscriber _transcriber;
        ElvishGenerator _generator;
        ElvishGenerator _helpGenerator;

        public ElvishTranslationService(string symbolsFilePath)
        {
            _transcriber = new ElvishTranscriber();
            _generator = new ElvishGenerator(symbolsFilePath, 12, false);
            _helpGenerator = new ElvishGenerator(symbolsFilePath, 12, true);
        }
        public string Transcribe(string input)
        {
            var result = _transcriber.Transcribe(input);
            return result;
        }

        public Kean.Xml.Dom.Document Generate(string input)
        {
            IEnumerable<char> textToGenerate = input;
            _generator.Append(textToGenerate);
            var result = _generator.GetPage();
            return result;
        }

        public Kean.Xml.Dom.Document GenerateWithTest(string input)
        {
            IEnumerable<char> textToGenerate = input;
            _helpGenerator.Append(textToGenerate);
            var result = _helpGenerator.GetPage();
            return result;
        }

        public Kean.Xml.Dom.Document TranscribeAndGenerate(string input)
        {
            var transcription = _transcriber.Transcribe(input);
            IEnumerable<char> textToGenerate = transcription;
            _generator.Append(textToGenerate);
            var result = _generator.GetPage();
            return result;
        }
        public Kean.Xml.Dom.Document TranscribeAndGenerate(string input, bool test)
        {
            var transcription = _transcriber.Transcribe(input);
            IEnumerable<char> textToGenerate = transcription;
            Kean.Xml.Dom.Document result;
            if (test)
            {
                _helpGenerator.Append(textToGenerate);
                result = _helpGenerator.GetPage();
            }
            else
            {
                _generator.Append(textToGenerate);
                result = _generator.GetPage();
            }
            return result;
        }
    }
}