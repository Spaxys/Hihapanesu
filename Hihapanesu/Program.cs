using System;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length > 0)
				foreach (string filename in args)
				{
					Console.Write(filename);
					string input = System.IO.File.ReadAllText(filename);
					Console.Write(".");
					string transcribed = new Transcriber().Transcribe(input);
					Console.Write(".");
					transcribed.Save(System.IO.Path.GetFileNameWithoutExtension(filename) + ".jp.txt");
					Console.Write(".");
					Generator g = new Generator();
					g.Append(transcribed);
					Console.Write(".");
					g.Save(Uri.Locator.FromRelativePlatformPath(System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));
					Console.WriteLine("done");
				}
			else
			{
				string input = Console.ReadLine();
				string transcribed = new Transcriber().Transcribe(input);
				Console.WriteLine();
				Console.WriteLine(transcribed);
				Generator g = new Generator();
				g.Append(transcribed);
				g.Save(Uri.Locator.FromRelativePlatformPath("test.svg"));
			}
		}
	}
}
