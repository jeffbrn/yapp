using Yapp.Parser.Grammer;

namespace Yapp.Tests.grammer {
	public class RuleTests {
		[Fact]
		public void Display() {
			Rule r = new("T", new Token("("), new("E"), new(")"));
			Assert.Equal("T -> . ( E ) ", r.ToString());
		}

		[Fact]
		public void Properties() {
			var tokens = new[] { new Token("("), new("E"), new(")") };
			Rule r = new("T", tokens);
			Assert.Equal("T", r.Name);
			Assert.Equal(3, r.Length);
			for (int i = 0; i < tokens.Length; i++) {
				Assert.Equal(tokens[i], r[i]);
			}
		}
	}
}
