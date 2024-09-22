using Yapp.Parser.Grammar;

namespace Yapp.Tests.grammar {
	public class RuleTests {
		[Fact]
		public void Display() {
			Rule r = new("T", 
				new RuleItem(new Token("(")),
				new("E"), 
				new(new Token(")")));
			Assert.Equal("T -> ( E ) ", r.ToString());
			Assert.Equal(11, r.StrLen);
		}

		[Fact]
		public void Properties() {
			var items = new[] { new RuleItem(new Token("(")), new("E"), new(new Token(")")) };
			Rule r = new("T", items);
			Assert.Equal("T", r.Name);
			Assert.Equal(3, r.Count);
			for (int i = 0; i < items.Length; i++) {
				Assert.Equal(items[i], r[i]);
			}
		}

		[Fact]
		public void Items() {
			Rule r = new("T",
				new RuleItem(new Token("(")),
				new("E"),
				new(new Token(")")));
			var tok_item = r[0];
			Assert.True(tok_item.IsTerminal);
			Assert.Equal("(", tok_item.Token.Name);
			Assert.Throws<InvalidOperationException>(() => tok_item.Rule);
			var rule_item = r[1];
			Assert.False(rule_item.IsTerminal);
			Assert.Equal("E", rule_item.Rule);
			Assert.Throws<InvalidOperationException>(() => rule_item.Token);
		}
	}
}
