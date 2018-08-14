using Hihapanesu.Web.Interfaces;
using Hihapanesu.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hihapanesu.Web.Tests.ServiceTests
{
    [TestClass]
    public class TranslationServiceTests
    {
        ITranslationService service;
        [TestInitialize]
        public void TestInitialize()
        {
            service = new ElvishTranslationService("elvishSymbols.svg");
        }

        [TestMethod]
        public void TestThatTranslateMethodShouldReturnAString()
        {
            var result = service.Transcribe("hej");
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void TestThatGenerateMethodShouldReturnNotNull()
        {
            var result = service.Generate("hej");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestThatGenerateMethodShouldReturnAnXmlDomDocument()
        {
            var result = service.Generate("hej");
            Assert.IsInstanceOfType(result, typeof(Kean.Xml.Dom.Document));
        }

        [TestMethod]
        public void TestThatGenerateWithTestMethodShouldReturnNotNull()
        {
            var result = service.GenerateWithTest("hej");
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void TestThatGenerateWithTestMethodShouldReturnAnXmlDomDocument()
        {
            var result = service.GenerateWithTest("hej");
            Assert.IsInstanceOfType(result, typeof(Kean.Xml.Dom.Document));
        }

        [TestMethod]
        public void TestThatTranscribeAndGenerateMethodShouldReturnAnXmlDomDocument()
        {
            var result = service.TranscribeAndGenerate("hej");
            Assert.IsInstanceOfType(result, typeof(Kean.Xml.Dom.Document));
        }

        [TestMethod]
        public void TestThatTranscribeAndGenerateMethodNoTestShouldReturnAnXmlDomDocument()
        {
            var result = service.TranscribeAndGenerate("hej", false);
            Assert.IsInstanceOfType(result, typeof(Kean.Xml.Dom.Document));
        }
        [TestMethod]
        public void TestThatTranscribeAndGenerateMethodYesTestShouldReturnAnXmlDomDocument()
        {
            var result = service.TranscribeAndGenerate("hej", true);
            Assert.IsInstanceOfType(result, typeof(Kean.Xml.Dom.Document));
        }
    }
}
