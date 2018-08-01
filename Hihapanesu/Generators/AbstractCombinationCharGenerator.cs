using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Xml = Kean.Xml;
using Uri = Kean.Core.Uri;
using Hihapanesu.Interfaces;
using System.Collections.Generic;
using Hihapanesu.Helpers;

namespace Hihapanesu.Generators
{
    public abstract class AbstractCombinationCharGenerator : IGenerator
    {
        private ICategorizer categorizer;
        protected string[] data = new string[19];

        protected string this[string character]
        {
            get
            {
                int key = this.Address(character);
                return this.data[key];
            }
            set
            {
                int key = this.Address(character);
                this.data[key] = value;
            }
        }

        //protected string this [string key]
        //{
        //	get { return this[key]; }
        //	set
        //	{ 
        //			this[key] = value;
        //	}
        //}

        public Geometry2D.Single.Size Feed { get; set; }

        public Geometry2D.Single.Size Offset { get; set; }

        public Geometry2D.Single.Size PageSize { get; set; }

        public bool Help { get; set; }

        protected Geometry2D.Single.Point position;
        protected Xml.Dom.Element root;
        private float charSize { get; set; }

        public AbstractCombinationCharGenerator()
        {
            categorizer = new ElvishCategorizer();
            charSize = 24;
            Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource("symbols.svg");
            this.Offset = new Geometry2D.Single.Size(2 * charSize, 3 * charSize);
            this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
            //this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
            this.Feed = new Geometry2D.Single.Size(charSize, charSize);
            //foreach (Xml.Dom.Node node in symbols.Root)
            //    if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
            //        this[(node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value[0]] = (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value;
            this.ResetPage();
        }

        public AbstractCombinationCharGenerator(string symbolsFileName, float charSize, bool helpActive = false)
        {
            categorizer = new ElvishCategorizer();
            this.charSize = charSize;
            Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource(symbolsFileName);
            this.Offset = new Geometry2D.Single.Size(2 * charSize, 2 *
                charSize);
            this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
            //this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
            this.Feed = new Geometry2D.Single.Size(charSize * 2, charSize * 3);
            //foreach (Xml.Dom.Node node in symbols.Root)
            //    if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
            //        this[(node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value[0]] = (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value;
            this.Help = helpActive;
            this.ResetPage();
        }

        protected bool ResetPage()
        {
            this.position = new Geometry2D.Single.Point();
            this.root = new Xml.Dom.Element("svg",
                                            KeyValue.Create("xmlns", "http://www.w3.org/2000/svg"),
                                            KeyValue.Create("version", "1.1"),
                                            KeyValue.Create("width", this.PageSize.Width.AsString()),
                                            KeyValue.Create("height", this.PageSize.Height.AsString()));

            //if (this.Help)
            //{
            //    while (this.position.X * this.Feed.Width < this.PageSize.Width && position.Y * this.Feed.Height < this.PageSize.Height)
            //    {
            //        Append(".");
            //    }
            //    this.position = new Geometry2D.Single.Point();
            //}
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
            string word = "";
            while (input.MoveNext())
            {
                char current = input.Current;
                if (current == ' ')
                {
                    AppendWord(word.GetEnumerator());
                    this.AppendWhitespace();
                    word = "";
                }
                else if (current == '_')
                {
                    AppendWord(word.GetEnumerator());
                    this.AppendNewLine();
                    word = "";
                }

                else
                {
                    word += current;
                }
            }
            if(word.Length > 0)
            {
                AppendWord(word.GetEnumerator());
            }
        }

        public void AppendWord(System.Collections.Generic.IEnumerator<char> input)
        {
            string character = "";
            LetterCategory inputCategory;
            LetterCategory previousCategory;
            char previous;
            char current = '\0';
            while (input.MoveNext())
            {
                previous = current;
                current = input.Current;
                previousCategory = categorizer.Categorize(previous);
                inputCategory = categorizer.Categorize(current);
                if (previousCategory != LetterCategory.Vowel && inputCategory == LetterCategory.Vowel)
                {
                    character += current;
                }
                else if(character == "")
                {
                    character += current;
                }
                else if(character.Length == 1)
                {
                    Append(character);
                    character = "";
                    character += current;
                }
                else
                {
                    Append(character);
                    character = "";
                }
                if (character.Length >= 2)
                {
                    Append(character);
                    character = "";
                }
            }
            if(character.Length > 0)
            {
                Append(character);
            }
        }
        /// <summary>
        /// Maps consonant and vocal character inputs to their respective int address output
        /// </summary>
        /// <param name="consonant"></param>
        /// <param name="vowel"></param>
        /// <returns></returns>
        /// 

        private Dictionary<string, int> Signs = new Dictionary<string, int>
        {

        };
		protected int Address(string character)
		{
			int c = 0;
            switch (character)
            {
                case "b":
                    c = 0;
                    break;
                case "d":
                    c = 1;
                    break;
                case "f":
                    c = 2;
                    break;
                case "g":
                    c = 3;
                    break;
                case "h":
                    c = 4;
                    break;
                case "k":
                    c = 5;
                    break;
                case "l":
                    c = 6;
                    break;
                case "m":
                    c = 7;
                    break;
                case "n":
                    c = 8;
                    break;
                case "p":
                    c = 9;
                    break;
                case "r":
                    c = 10;
                    break;
                case "s":
                    c = 11;
                    break;
                case "t":
                    c = 12;
                    break;
                case "v":
                    c = 13;
                    break;
                case "a":
                    c = 14;
                    break;
                case "e":
                    c = 1;
                    break;
                case "i":
                    c = 2;
                    break;
                case "o":
                    c = 3;
                    break;
                case "u":
                    c = 4;
                    break;
            }
			return c;
		}

        /// <summary>
        /// Appends a child element to the root xml document <para />
        /// Always places a transcibed custom svg character, and optionally the free text beside it. 
        /// </summary>
        /// <param name="consonant"></param>
        /// <param name="vowel"></param>
		public void Append(string character)
		{
			Geometry2D.Single.Point translate;
            if (this.Help)
            {
                translate = this.position * this.Feed + this.Offset; 
                //Adds a '(' character before the input string
               this.root.Add(new Xml.Dom.Element("text",
                                              new Xml.Dom.Text(new string( new char[] { '(' } )),
                                              KeyValue.Create("style", "text-anchor: right; font-family: Times New Roman; font-size: "+ charSize +"mm"),
                                              KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
                                              ));
                this.Move(0.2f);
            }

            //Append all characters in the string vertically, starting from the bottom
            var index = 0; 
            translate = this.position * this.Feed + this.Offset; 
            foreach(var c in character)
            {
                this.root.Add(new Xml.Dom.Element("text",
                                                  new Xml.Dom.Text(new string(new char[] { c })),
                                                  KeyValue.Create("style", "text-anchor: right; font-family: Times New Roman; font-size: " + charSize + "mm"),
                                                  //KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
				                                  KeyValue.Create("transform", "translate(" + (translate.X + this.Feed.Width).AsString() + ", " + (translate.Y + this.Feed.Height - this.Feed.Height*index).AsString() + ")")
                                                  ));
                index++;
            }

			if (this.Help)
                //Adds a '.' character to visualize the space taken on the page
                //Adds an xml child text element to the root element. Just for testing purposes I guess, free text of the transcribed text I guess
				this.root.Add(new Xml.Dom.Element("text", 
				                                  new Xml.Dom.Text(new string(character.ToCharArray() )),
				                                  KeyValue.Create("style", "text-anchor: right; font: Verdana 10pt"),
				                                  KeyValue.Create("transform", "translate(" + (translate.X + this.Feed.Width - 10).AsString() + ", " + (translate.Y + this.Feed.Height / 2).AsString() + ")")
				)
				);
            if (this.Help)
            {
                translate = this.position * this.Feed + this.Offset; 
                //Adds a ')' character before the input string
                this.root.Add(new Xml.Dom.Element("text",
                                              new Xml.Dom.Text(new string( new char[] { ')' } )),
                                              KeyValue.Create("style", "text-anchor: right; font-family: Times New Roman; font-size: "+ charSize +"mm"),
                                              KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
                                              ));
                this.Move(0.2f);
            }
			this.Move(1.0f);
		}

        public abstract void AppendWhitespace();
        public abstract void AppendNewLine();

        protected abstract void Move(float distance);

		public bool Save(Uri.Locator resource)
		{
			return new Xml.Dom.Document(this.root).Save(resource) && this.ResetPage();
		}
	}
}

