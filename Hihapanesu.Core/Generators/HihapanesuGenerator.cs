using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;
using Hihapanesu.Interfaces;

namespace Hihapanesu.Generators
{
	public class HihapanesuGenerator : AbstractVerticalGenerator, IGenerator
    {
        public HihapanesuGenerator()
		{
			Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource("symbols.svg");
			this.Offset = new Geometry2D.Single.Size(32, 32);
			this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
			this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
			foreach (Xml.Dom.Node node in symbols.Root)
				if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
					this[(node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value] = (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value;
			this.ResetPage();
		}
        public HihapanesuGenerator(string symbolsFileName)
		{
			Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource(symbolsFileName);
			this.Offset = new Geometry2D.Single.Size(32, 32);
			this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
			this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
			foreach (Xml.Dom.Node node in symbols.Root)
				if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
					this[(node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value] = (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value;
			this.ResetPage();
		}

    }
}

