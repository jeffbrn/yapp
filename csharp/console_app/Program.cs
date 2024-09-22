namespace Yapp.Console;

using System;
using Parser.Grammar;

internal class Program {
	private static void Main(string[] args) {
		RuleCollection grammer = new("S'", "E");
		grammer.Add("E", "E-T");
		grammer.Add("E", "T");
		grammer.Add("T", "n");
		grammer.Add("T", "(E)");
		Console.WriteLine("Grammar :-");
		Console.WriteLine("================================");
		Console.WriteLine(grammer.ToString());
	}
}