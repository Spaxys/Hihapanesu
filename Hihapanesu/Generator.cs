using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	public class Generator
	{
		string[,] data = new string[14, 5];

		string this [char consonant, char vowel]
		{
			get
			{ 
				Tuple<int, int> key = this.Address(consonant, vowel);
				return this.data[key.Item1, key.Item2];
			}
			set
			{ 
				Tuple<int, int> key = this.Address(consonant, vowel);
				this.data[key.Item1, key.Item2] = value;
			}
		}

		string this [string key]
		{
			get { return key.Length > 1 ? this[key[0], key[1]] : null; }
			set
			{ 
				if (key.Length > 1)
					this[key[0], key[1]] = value;
			}
		}

		public Geometry2D.Single.Size Feed { get; set; }

		public Geometry2D.Single.Size Offset { get; set; }

		public Geometry2D.Single.Size PageSize { get; set; }

		public bool Help { get; set; }

		Geometry2D.Single.Point position;
		Xml.Dom.Element root;

		public Generator()
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

		bool ResetPage()
		{
			this.position = new Geometry2D.Single.Point();
			this.root = new Xml.Dom.Element("svg",
			                                KeyValue.Create("xmlns", "http://www.w3.org/2000/svg"),
			                                KeyValue.Create("version", "1.1"),
			                                KeyValue.Create("width", this.PageSize.Width.AsString()),
			                                KeyValue.Create("height", this.PageSize.Height.AsString()));
			return true;
		}

        /// <summary>
        /// Takes an IEnumerable-char input, and calls Append(...) on each character, appending a child element for each character to a root xml document.
        /// </summary>
        /// <param name="input"></param>
		public void Append(System.Collections.Generic.IEnumerable<char> input)
		{
			this.Append(input.GetEnumerator());
		}

        /// <summary>
        /// Takes any character, or a whitespace, and calls the respective appropriate append method.
        /// </summary>
        /// <param name="input"></param>
		public void Append(System.Collections.Generic.IEnumerator<char> input)
		{
			while (input.MoveNext())
			{
				char current = input.Current;
				if (current == ' ')
					this.AppendWhitespace();
				else if (input.MoveNext())
					this.Append(current, input.Current);
			}
		}

        /// <summary>
        /// Maps consonant and vocal character inputs to their respective int address output
        /// </summary>
        /// <param name="consonant"></param>
        /// <param name="vowel"></param>
        /// <returns></returns>
		Tuple<int, int> Address(char consonant, char vowel)
		{
			int c = 0;
			switch (consonant)
			{
				case 'b':
					c = 0;
					break;
				case 'd':
					c = 1;
					break;
				case 'f':
					c = 2;
					break;
				case 'g':
					c = 3;
					break;
				case 'h':
					c = 4;
					break;
				case 'k':
					c = 5;
					break;
				case 'l':
					c = 6;
					break;
				case 'm':
					c = 7;
					break;
				case 'n':
					c = 8;
					break;
				case 'p':
					c = 9;
					break;
				case 'r':
					c = 10;
					break;
				case 's':
					c = 11;
					break;
				case 't':
					c = 12;
					break;
				case 'v':
					c = 13;
					break;
			}
			int v = 0;
			switch (vowel)
			{
				case 'a':
					v = 0;
					break;
				case 'e':
					v = 1;
					break;
				case 'i':
					v = 2;
					break;
				case 'o':
					v = 3;
					break;
				case 'u':
					v = 4;
					break;
			}
			return Tuple.Create(c, v);
		}

        /// <summary>
        /// Appends a child element to the root xml document <para />
        /// Always places a transcibed custom svg character, and optionally the free text beside it. 
        /// </summary>
        /// <param name="consonant"></param>
        /// <param name="vowel"></param>
		public void Append(char consonant, char vowel)
		{
			Geometry2D.Single.Point translate = this.position * this.Feed + this.Offset;
            //Adds an xml child element with the transcribed character svg(?) to the root element
			this.root.Add(new Xml.Dom.Element("path", 
			                                  KeyValue.Create("d", this[consonant, vowel]), 
			                                  KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
			)
			);
			if (true || this.Help)
                //Adds an xml child text element to the root element. Just for testing purposes I guess, free text of the transcribed text I guess
				this.root.Add(new Xml.Dom.Element("text", 
				                                  new Xml.Dom.Text(new string(new char[] { consonant, vowel })),
				                                  KeyValue.Create("style", "text-anchor: right; font: Verdana 10pt"),
				                                  KeyValue.Create("transform", "translate(" + (translate.X + this.Feed.Width - 10).AsString() + ", " + (translate.Y + this.Feed.Height / 2).AsString() + ")")
				)
				);

			this.Move(1.0f);
		}

		public void AppendWhitespace()
		{
			if (this.position.Y != 0)
				this.Move(0.5f);
		}

		void Move(float distance)
		{
			this.position += new Geometry2D.Single.Point(0, distance);
			Geometry2D.Single.Point totalSize = this.position * this.Feed + this.Feed + 2 * this.Offset;
			if (totalSize.Y >= this.PageSize.Height)
				this.position = new Geometry2D.Single.Point(this.position.X + 1, 0);
		}

		public bool Save(Uri.Locator resource)
		{
			return new Xml.Dom.Document(this.root).Save(resource) && this.ResetPage();
		}
	}
}

