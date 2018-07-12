using System;
using Hihapanesu.Generators;
using Hihapanesu.Interfaces;
using Hihapanesu.Transcribers;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            IGenerator g;
            ITranscriber t;
            if (args.Length > 0)
            {
                g = new HihapanesuGenerator(); 
                t = new HihapanesuTranscriber();
                foreach (string filename in args)
                {
                    Console.Write(filename);
                    string input = System.IO.File.ReadAllText(filename).ToLower();
                    Console.Write(".");
                    string transcribed = t.Transcribe(input);
                    Console.Write(".");
                    transcribed.Save(System.IO.Path.GetFileNameWithoutExtension(filename) + ".jp.txt");
                    Console.Write(".");
                    g.Append(transcribed);
                    Console.Write(".");
                    g.Save(Uri.Locator.FromRelativePlatformPath(System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));
                    Console.WriteLine("done");
                }
            }
            else
            {
                Console.WriteLine("Select a transcriber and press Enter");
                Console.WriteLine("0: Hihapanesu transcriber");
                Console.WriteLine("1: Elvish transcriber");
                var transcriberChoice = Console.ReadLine();
                
                Console.WriteLine("Select vertical or horizontal generation and press Enter");
                Console.WriteLine("0: Vertical");
                Console.WriteLine("1: Horizontal");
                var generationChoise = Console.ReadLine();

                
                switch(int.Parse(transcriberChoice))
                {
                    case 1:
                        t = new ElvishTranscriber();
                        //Select horizontal or vertical generator based on input
                        switch(int.Parse(generationChoise))
                        {
                            default:
                                g = new ElvishGenerator("symbols.svg", 12f, true);
                                break;
                        }
                        break;
                    case 0:
                    default:
                        t = new HihapanesuTranscriber();
                        //Select horizontal or vertical generator based on input
                        switch(int.Parse(generationChoise))
                        {
                            case 1:
                                g = new HihapanesuGeneratorHorizontal();
                                break;
                            case 0:
                            default:
                                g = new HihapanesuGenerator();
                                break;
                        }
                        break;
                }

                Console.WriteLine("Write your input text below:");
                string input = Console.ReadLine().ToLower();
                string transcribed = t.Transcribe(input);
                Console.WriteLine();
                Console.WriteLine(transcribed);
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
                //if (!(g is ElvishGenerator))
                //{
                    g.Append(transcribed);
                    g.Save(Uri.Locator.FromRelativePlatformPath("test.svg"));
                //}
            }
		}
	}
}
