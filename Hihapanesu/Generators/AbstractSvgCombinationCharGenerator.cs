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
    public abstract class AbstractSvgCombinationCharGenerator : IGenerator
    {
        private ICategorizer categorizer;
        //protected string[] data = new string[150];
        protected Dictionary<string, string> data = new Dictionary<string, string>();

        protected string this[string character]
        {
            get
            {
                //int key = this.Address(character);
                //return this.data[key];
                return this.data[character];
            }
            set
            {
                //int key = this.Address(character);
                //this.data[key] = value;
                this.data[character] = value;
            }
        }

        public Geometry2D.Single.Size Feed { get; set; }

        public Geometry2D.Single.Size Offset { get; set; }

        public Geometry2D.Single.Size PageSize { get; set; }

        public bool Help { get; set; }

        protected Geometry2D.Single.Point position;
        protected Xml.Dom.Element root;
        private float charSize { get; set; }

        public AbstractSvgCombinationCharGenerator()
        {
            categorizer = new ElvishCategorizer();
            charSize = 24;
            Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource("symbols.svg");
            this.Offset = new Geometry2D.Single.Size(2 * charSize, 3 * charSize);
            this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
            //this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
            this.Feed = new Geometry2D.Single.Size(charSize, charSize);
            foreach (Xml.Dom.Node node in symbols.Root)
                if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
                {
                    data.Add((node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value, (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value);
                }
            this.ResetPage();
        }

        public AbstractSvgCombinationCharGenerator(string symbolsFileName, float charSize, bool helpActive = false)
        {
            categorizer = new ElvishCategorizer();
            this.charSize = charSize;
            Xml.Dom.Document symbols = Xml.Dom.Document.OpenResource(symbolsFileName);
            this.Offset = new Geometry2D.Single.Size(2 * charSize, 2 *
                charSize);
            this.PageSize = new Geometry2D.Single.Size(744.09f, 1052.36f);
            //this.Feed = new Geometry2D.Single.Size(symbols.Root.Attributes.Find(a => a.Name == "width").Value.Parse<float>(), symbols.Root.Attributes.Find(a => a.Name == "height").Value.Parse<float>());
            this.Feed = new Geometry2D.Single.Size(charSize * 2, charSize * 3);
            foreach (Xml.Dom.Node node in symbols.Root)
                if (node is Xml.Dom.Element && (node as Xml.Dom.Element).Name == "path")
                {
                    data.Add((node as Xml.Dom.Element).Attributes.Find(a => a.Name == "id").Value, (node as Xml.Dom.Element).Attributes.Find(a => a.Name == "d").Value);
                }
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
            var charactersToAppend = new List<string>();
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
                    charactersToAppend.Add(character);
                    //Append(character);
                    character = "";
                    character += current;
                }
                else
                {
                    charactersToAppend.Add(character);
                    //Append(character);
                    character = "";
                }
                if (character.Length >= 2)
                {
                    charactersToAppend.Add(character);
                    //Append(character);
                    character = "";
                }
            }
            if(character.Length > 0)
            {
                    charactersToAppend.Add(character);
                //Append(character);
            }
            //Add a newline if this word does not fit on this line
            var currentPosition = this.position * this.Feed + this.Offset; 
            if(currentPosition.X + this.Feed.Width*charactersToAppend.Count > PageSize.Width)
            {
                AppendNewLine();
            }
            foreach(var c in charactersToAppend)
            {
                Append(c);
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
                    case "j":
                        c = 5;
                        break;
                    case "k":
                        c = 6;
                        break;
                    case "l":
                        c = 7;
                        break;
                    case "m":
                        c = 8;
                        break;
                    case "n":
                        c = 9;
                        break;
                    case "p":
                        c = 10;
                        break;
                    case "r":
                        c = 11;
                        break;
                    case "s":
                        c = 12;
                        break;
                    case "t":
                        c = 13;
                        break;
                    case "v":
                        c = 14;
                        break;
                    case "a":
                        c = 15;
                        break;
                    case "e":
                        c = 16;
                        break;
                    case "i":
                        c = 17;
                        break;
                    case "o":
                        c = 18;
                        break;
                    case "u":
                        c = 19;
                        break;

                    //Combination characters

                    //B:s
                     case "ba":
                        c = 20;
                    break;
                     case "be":
                        c = 21;
                        break;
                     case "bi":
                        c = 22;
                        break;
                     case "bo":
                        c = 23;
                        break;
                     case "bu":
                        c = 24;
                        break;
                     case "by":
                        c = 25;
                        break;

                    //D:s
                    case "da":
                        c = 26;
                        break;
                    case "de":
                        c = 27;
                        break;
                    case "di":
                        c = 28;
                        break;
                    case "do":
                        c = 29;
                        break;
                    case "du":
                        c = 30;
                        break;
                    case "dy":
                        c = 31;
                        break;

                    //F:s
                    case "fa":
                        c = 32;
                        break;
                    case "fe":
                        c = 33;
                        break;
                    case "fi":
                        c = 34;
                        break;
                    case "fo":
                        c = 35;
                        break;
                    case "fu":
                        c = 36;
                        break;
                    case "fy":
                        c = 37;
                        break;

                    //G:s
                    case "ga":
                        c = 38;
                        break;
                    case "ge":
                        c = 39;
                        break;
                    case "gi":
                        c = 40;
                        break;
                    case "go":
                        c = 41;
                        break;
                    case "gu":
                        c = 42;
                        break;
                    case "gy":
                        c = 43;
                        break;

                    //H:s
                    case "ha":
                        c = 44;
                        break;
                    case "he":
                        c = 45;
                        break;
                    case "hi":
                        c = 46;
                        break;
                    case "ho":
                        c = 47;
                        break;
                    case "hu":
                        c = 48;
                        break;
                    case "hy":
                        c = 49;
                        break;

                    //J:s
                    case "ja":
                        c = 50;
                    break;
                    case "je":
                        c = 51;
                    break;
                    case "ji":
                        c = 52;
                    break;
                    case "jo":
                        c = 53;
                    break;
                    case "ju":
                        c = 54;
                    break;
                    case "jy":
                        c = 55;
                    break;

                    //K:s
                    case "ka":
                        c = 56;
                        break;
                    case "ke":
                        c = 57;
                        break;
                    case "ki":
                        c = 58;
                        break;
                    case "ko":
                        c = 59;
                        break;
                    case "ku":
                        c = 60;
                        break;
                    case "ky":
                        c = 61;
                        break;

                    //L:s
                    case "la":
                        c = 62;
                        break;
                    case "le":
                        c = 63;
                        break;
                    case "li":
                        c = 64;
                        break;
                    case "lo":
                        c = 65;
                        break;
                    case "lu":
                        c = 66;
                        break;
                    case "ly":
                        c = 67;
                        break;

                    //M:s
                    case "ma":
                        c = 68;
                        break;
                    case "me":
                        c = 69;
                        break;
                    case "mi":
                        c = 70;
                        break;
                    case "mo":
                        c = 71;
                        break;
                    case "mu":
                        c = 72;
                        break;
                    case "my":
                        c = 73;
                        break;

                    //N:s
                    case "na":
                        c = 74;
                        break;
                    case "ne":
                        c = 75;
                        break;
                    case "ni":
                        c = 76;
                        break;
                    case "no":
                        c = 77;
                        break;
                    case "nu":
                        c = 78;
                        break;
                    case "ny":
                        c = 79;
                        break;

                    //P:s
                    case "pa":
                        c = 80;
                        break;
                    case "pe":
                        c = 81;
                        break;
                    case "pi":
                        c = 82;
                        break;
                    case "po":
                        c = 83;
                        break;
                    case "pu":
                        c = 84;
                        break;
                    case "py":
                        c = 85;
                        break;

                    //R:s
                    case "ra":
                        c = 86;
                        break;
                    case "re":
                        c = 87;
                        break;
                    case "ri":
                        c = 88;
                        break;
                    case "ro":
                        c = 89;
                        break;
                    case "ru":
                        c = 90;
                        break;
                    case "ry":
                        c = 91;
                        break;

                    //S:s
                    case "sa":
                        c = 92;
                        break;
                    case "se":
                        c = 93;
                        break;
                    case "si":
                        c = 94;
                        break;
                    case "so":
                        c = 95;
                        break;
                    case "su":
                        c = 96;
                        break;
                    case "sy":
                        c = 97;
                        break;

                    //T:s
                    case "ta":
                        c = 98;
                        break;
                    case "te":
                        c = 99;
                        break;
                    case "ti":
                        c = 100;
                        break;
                    case "to":
                        c = 101;
                        break;
                    case "tu":
                        c = 102;
                        break;
                    case "ty":
                        c = 103;
                        break;

                    //V:s
                    case "va":
                        c = 104;
                    break;
                    case "ve":
                        c = 105;
                    break;
                    case "vi":
                        c = 106;
                    break;
                    case "vo":
                        c = 107;
                    break;
                    case "vu":
                        c = 108;
                    break;
                    case "vy":
                        c = 109;
                    break;

                    //X:s
                    case "xa":
                        c = 110;
                    break;
                    case "xe":
                        c = 111;
                    break;
                    case "xi":
                        c = 112;
                    break;
                    case "xo":
                        c = 113;
                    break;
                    case "xu":
                        c = 114;
                    break;
                    case "xy":
                        c = 115;
                    break;

                    //Z:s
                    case "za":
                        c = 116;
                    break;
                    case "ze":
                        c = 117;
                    break;
                    case "zi":
                        c = 118;
                    break;
                    case "zo":
                        c = 119;
                    break;
                    case "zu":
                        c = 120;
                    break;
                    case "zy":
                        c = 121;
                    break;

                    //C:s
                    case "ca":
                        c = 122;
                    break;
                    case "ce":
                        c = 123;
                    break;
                    case "ci":
                        c = 124;
                    break;
                    case "co":
                        c = 125;
                    break;
                    case "cu":
                        c = 126;
                    break;
                    case "cy":
                        c = 127;
                    break;

                    //Q:s
                    case "qa":
                        c = 128;
                    break;
                    case "qe":
                        c = 129;
                    break;
                    case "qi":
                        c = 130;
                    break;
                    case "qo":
                        c = 131;
                    break;
                    case "qu":
                        c = 132;
                    break;
                    case "qy":
                        c = 133;
                    break;

                    //W:s
                    case "wa":
                        c = 134;
                    break;
                    case "we":
                        c = 135;
                    break;
                    case "wi":
                        c = 136;
                    break;
                    case "wo":
                        c = 137;
                    break;
                    case "wu":
                        c = 138;
                    break;
                    case "wy":
                        c = 139;
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

            //Append all characters in the string vertically, starting from the bottom
            var index = 0;
            var succeededGettingPrintableChar = false;
            string printableChar = "";
            try
            {
                printableChar = this[character];
                succeededGettingPrintableChar = true;
            }
            catch
            {}
            translate = this.position * this.Feed + this.Offset;

            if (succeededGettingPrintableChar)
            {
                this.root.Add(new Xml.Dom.Element("path",
                                                  KeyValue.Create("d", printableChar),
                                                  KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
            )
            );
            }
            else
            {
				this.root.Add(new Xml.Dom.Element("text", 
				                                  new Xml.Dom.Text(new string(character.ToCharArray() )),
				                                  KeyValue.Create("style", "text-anchor: right; font: Verdana 10pt"),
                                                  KeyValue.Create("transform", "translate(" + translate.ToString() + ")")
				)
				);

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

