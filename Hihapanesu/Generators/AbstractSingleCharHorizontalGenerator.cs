using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;
using Hihapanesu.Interfaces;

namespace Hihapanesu.Generators
{
	public abstract class AbstractHorizontalSingleCharGenerator : AbstractSingleCharGenerator, IGenerator
    {

		public AbstractHorizontalSingleCharGenerator()
            : base() {}

        public AbstractHorizontalSingleCharGenerator(string symbolsFileName, float charSize, bool useHelp)
            : base(symbolsFileName, charSize, useHelp) {}

		protected override void Move(float distance)
		{
			this.position += new Geometry2D.Single.Point(distance, 0);
			Geometry2D.Single.Point totalSize = this.position * this.Feed + this.Feed + 2 * this.Offset;
			if (totalSize.X >= this.PageSize.Width)
				this.position = new Geometry2D.Single.Point(0, this.position.Y + 1);
		}
		public override void AppendWhitespace()
		{
			if (this.position.X != 0)
				this.Move(0.5f);
		}
		public override void AppendNewLine()
		{
			this.position = new Geometry2D.Single.Point(0, this.position.Y + 1);
		}
	}
}

