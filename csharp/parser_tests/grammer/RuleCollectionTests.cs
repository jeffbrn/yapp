using Yapp.Parser.Grammer;

namespace Yapp.Tests.grammer {
	public class RuleCollectionTests {
		[Fact]
		public void Display() {
			RuleCollection grm = new("A", "z");
			grm.Add("B", "b");
			var expected = @"A -> z $ 
B -> b 
";
			Assert.Equal(expected, grm.ToString());
		}

		[Fact]
		public void Properties() {
			RuleCollection grm = new("A", "z");
			Assert.Equal("A", grm.StartRule.Name);
		}

		[Fact]
		public void CheckRules() {
			RuleCollection grm = new("A", "Az");
			var r = grm.StartRule;
			Assert.Equal(3, r.Count);
			Assert.False(r[0].IsTerminal);
			Assert.Equal("A", r[0].Rule);
			Assert.True(r[1].IsTerminal);
			Assert.Equal("z", r[1].Token.Name);
			Assert.True(r[2].IsTerminal);
			Assert.Equal("$", r[2].Token.Name);
		}

		[Fact]
		public void CheckDisplayWidth() {
			RuleCollection rules = new("S'", "S");
			rules.Add("S", "AA");
			rules.Add("A", "aAx");
			rules.Add("A", "b");
			Assert.Equal(rules[2].StrLen, rules.DisplayWidth);
		}
	}
}
