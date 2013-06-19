using System;
using Uri = Kean.Core.Uri;

namespace Hihapanesu
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Transcriber t = new Transcriber();
			Generator g = new Generator();
			g.Append(t.Transcribe(Console.ReadLine()));
			g.Save(Uri.Locator.FromRelativePlatformPath("test.svg"));
			System.Diagnostics.Process.Start("test.svg");
		}
	}
}
