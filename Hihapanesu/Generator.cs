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

		Geometry2D.Single.Point position = new Geometry2D.Single.Point();
		Xml.Dom.Element root = new Xml.Dom.Element("svg");

		public Generator()
		{
			Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource("symbols.svg");
			this.Offset = new Geometry2D.Single.Size(10, 10);
			this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
			foreach (Xml.Dom.Node node in symbols.Root)
				if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
					this[(node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value] = (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value;
		}

		public void Append(System.Collections.Generic.IEnumerable<char> input)
		{
			this.Append(input.GetEnumerator());
		}

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

		public void Append(char consonant, char vowel)
		{
			Geometry2D.Single.Point translate = this.position * this.Feed + this.Offset;
			this.root.Add(new Xml.Dom.Element("path", 
			                                  KeyValue.Create("d", this[consonant, vowel]), 
			                                  KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
			)
			);
			this.Move();
		}

		public void AppendWhitespace()
		{
			this.Move();
		}

		void Move()
		{
			if (this.position.Y > 14)
				this.position = new Geometry2D.Single.Point(this.position.X + 1, 0);
			else
				this.position += new Geometry2D.Single.Point(0, 1);
		}

		public bool Save(Uri.Locator resource)
		{
			return new Xml.Dom.Document(this.root).Save(resource);
		}
	}
}

