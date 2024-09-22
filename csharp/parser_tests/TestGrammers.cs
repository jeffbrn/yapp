using System;
using Yapp.Parser.Grammar;

namespace Yapp.Tests {

	internal static class TestGrammars {
		internal static RuleCollection Grammar1() {
			RuleCollection rules = new("S'", "S") {
				{ "S", "AA" },
				{ "A", "aA" },
				{ "A", "b" }
			};
			return rules;
		}

		internal static RuleCollection Grammar2() {
			RuleCollection rules = new("S'", "E") {
				{ "E", "E-T" },
				{ "E", "T" },
				{ "T", "n" },
				{ "T", "(E)" }
			};
			return rules;
		}
	}

}
