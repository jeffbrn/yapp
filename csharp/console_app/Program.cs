namespace Yapp.Console;

using System;
using Parser.Grammer;

internal class Program {
	private static void Main(string[] args) {
		RuleCollection grammer = new("S'", "E");
		grammer.Add("E", "E-T");
		grammer.Add("E", "T");
		grammer.Add("T", "n");
		grammer.Add("T", "(E)");
		Console.WriteLine("Grammer :-");
		Console.WriteLine("================================");
		Console.WriteLine(grammer.ToString());
	}
}