using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;
using Hihapanesu.Interfaces;

namespace Hihapanesu.Generators
{
	public abstract class AbstractVerticalGenerator : AbstractGenerator, IGenerator
    {

		public AbstractVerticalGenerator()
            : base() {}

        public AbstractVerticalGenerator(string symbolsFileName)
            : base(symbolsFileName) {}

		protected override void Move(float distance)
		{
			this.position += new Geometry2D.Single.Point(0, distance);
			Geometry2D.Single.Point totalSize = this.position * this.Feed + this.Feed + 2 * this.Offset;
			if (totalSize.Y >= this.PageSize.Height)
				this.position = new Geometry2D.Single.Point(this.position.X + 1, 0);
		}
		public override void AppendWhitespace()
		{
			if (this.position.Y != 0)
				this.Move(0.5f);
		}
	}
}

