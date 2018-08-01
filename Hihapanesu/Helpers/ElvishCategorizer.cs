using Hihapanesu.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hihapanesu.Helpers
{
    public class ElvishCategorizer : ICategorizer
    {
        public LetterCategory Categorize(char c)
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
    }
}
