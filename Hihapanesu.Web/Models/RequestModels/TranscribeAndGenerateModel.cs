using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hihapanesu.Web.Models.RequestModels
{
    public class TranscribeAndGenerateModel
    {
        public string Text { get; set; }
        public bool IsTest { get; set; } = false;
    }
}