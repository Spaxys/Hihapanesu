using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;
using Hihapanesu.Interfaces;

namespace Hihapanesu.Generators
{
	public class ElvishGenerator : AbstractHorizontalSingleCharGenerator, IGenerator
    {

        public ElvishGenerator()
            : base()		{
		}

        public ElvishGenerator(string symbolsFileName, float charSize, bool useHelp)
            :base(symbolsFileName, charSize, useHelp) { }

	}
}

