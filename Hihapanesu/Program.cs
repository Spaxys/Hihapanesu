using System;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string input = Console.ReadLine();
			string transcribed = new Transcriber().Transcribe(input);

			(input + "\n\n----------------------------------------------------------------------------\n\n" + transcribed).
				Save("test.txt");

			Console.WriteLine();
			Console.WriteLine(transcribed);

			Generator g = new Generator();
			g.Append(transcribed);
			g.Save(Uri.Locator.FromRelativePlatformPath("test.svg"));
			System.Diagnostics.Process.Start("test.svg");
		}
	}
}
