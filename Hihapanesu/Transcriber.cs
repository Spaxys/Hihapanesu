using System;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;

namespace Hihapanesu
{
	public class Transcriber
	{
		public Transcriber()
		{
		}

		char Map(char c)
		{
			char result;
			switch (c)
			{
				case 'c':
					result = 'k';
					break;
				case 'j':
					result = 'i';
					break;
				case 'w':
					result = 'v';
					break;
				case 'x':
					result = 's';
					break;
				case 'y':
					result = 'i';
					break;
				case 'z':
					result = 's';
					break;
				case 'å':
					result = 'o';
					break;
				case 'ä':
					result = 'e';
					break;
				case 'ö':
					result = 'u';
					break;
				default:
					result = c;
					break;
			}
			return result;
		}

		LetterCategory Categorize(char c)
		{
			c = char.ToLower(c);
			LetterCategory result;
			switch (c)
			{
				default:
					result = LetterCategory.Other;
					break;
				case 'a':
				case 'e':
				case 'i':
				case 'o':
				case 'u':
				case 'y':
				case 'å':
				case 'ä':
				case 'ö':
					result = LetterCategory.Vowel;
					break;
				case 'b':
				case 'c':
				case 'd':
				case 'f':
				case 'g':
				case 'h':
				case 'j':
				case 'k':
				case 'l':
				case 'm':
				case 'n':
				case 'p':
				case 'q':
				case 'r':
				case 's':
				case 't':
				case 'v':
				case 'w':
				case 'x':
				case 'z':
					result = LetterCategory.Consonant;
					break;
				case ' ':
				case '\n':
				case '\r':
				case '\t':
					result = LetterCategory.Whitespace;
					break;
			}
			return result;
		}

		public string Transcribe(string input)
		{
			return this.Transcribe(input.ToCharArray()).Fold((c, s) => s + c, "");
		}

		public System.Collections.Generic.IEnumerable<char> Transcribe(System.Collections.Generic.IEnumerable<char> input)
		{
			LetterCategory lastCategory = LetterCategory.Whitespace;
			System.Collections.Generic.IEnumerator<char> enumerator = input.GetEnumerator();
			bool evenInsertVowel = false;
			if (!enumerator.MoveNext())
				yield break;
			bool done = false;
			while (!done)
			{
				char current = enumerator.Current;
				if (!enumerator.MoveNext())
					done = true;
				else if (current == enumerator.Current)
					done = !enumerator.MoveNext();
				else if (current == 'c' && enumerator.Current == 'k')
				{
					done = !enumerator.MoveNext();
					current = 'k';
				}
				else if (current == 'c' && enumerator.Current == 'h')
				{
					done = !enumerator.MoveNext();
					current = lastCategory == LetterCategory.Vowel ? 'k' : 's';
				}
				current = this.Map(current);
				LetterCategory category = this.Categorize(current);
				switch (category)
				{
					case LetterCategory.Other:
						break;
					case LetterCategory.Vowel:
						if (lastCategory != LetterCategory.Consonant)
							yield return 'h';
						yield return current;
						break;
					case LetterCategory.Consonant:
						if (lastCategory == LetterCategory.Consonant)
							yield return (evenInsertVowel = !evenInsertVowel) ? 'u' : 'i';
						yield return current;
						break;
					case LetterCategory.Whitespace:
						if (lastCategory == LetterCategory.Consonant)
							yield return !evenInsertVowel ? 'u' : 'i';
						evenInsertVowel = false;
						if (lastCategory != LetterCategory.Whitespace)
							yield return ' ';
						break;
				}
				if (category != LetterCategory.Other)
					lastCategory = category;
			}
			if (lastCategory == LetterCategory.Consonant)
				yield return (evenInsertVowel = !evenInsertVowel) ? 'u' : 'i';
		}
	}
}

