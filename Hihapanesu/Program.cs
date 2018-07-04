using System;
using Hihapanesu.Transcribers;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Generator g = new Generator();
			if (args.Length > 0)
				foreach (string filename in args)
				{
					Console.Write(filename);
					string input = System.IO.File.ReadAllText(filename).ToLower();
					Console.Write(".");
					string transcribed = new Transcriber().Transcribe(input);
					Console.Write(".");
					transcribed.Save(System.IO.Path.GetFileNameWithoutExtension(filename) + ".jp.txt");
					Console.Write(".");
					g.Append(transcribed);
					Console.Write(".");
					g.Save(Uri.Locator.FromRelativePlatformPath(System.IO.Path.GetFileNameWithoutExtension(filename) + ".svg"));
					Console.WriteLine("done");
				}
			else
			{
				string input = Console.ReadLine().ToLower();
				string transcribed = new HihapanesuTranscriber().Transcribe(input);
				Console.WriteLine();
				Console.WriteLine(transcribed);
				g.Append(transcribed);
				g.Save(Uri.Locator.FromRelativePlatformPath("test.svg"));
			}
		}
	}
}
