using Yapp.Parser.Grammer;

namespace Yapp.Tests.grammer {
	public class RuleCollectionTests {
		[Fact]
		public void Display() {
			RuleCollection grm = new("A", "z");
			grm.Add("B", "b");
			var expected = @"A -> . z 
B -> . b 
";
			Assert.Equal(expected, grm.ToString());
		}
	}
}
