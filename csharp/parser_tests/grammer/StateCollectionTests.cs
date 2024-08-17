using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Yapp.Parser.Grammer;
using Yapp.Parser.Lalr;

namespace Yapp.Tests.grammer {
	public class StateCollectionTests {
		private readonly ITestOutputHelper _test_output_helper;
		private readonly RuleCollection _rules;

		public StateCollectionTests(ITestOutputHelper testOutputHelper) {
			_test_output_helper = testOutputHelper;
			_rules = new("S'", "S");
			_rules.Add("S", "AA");
			_rules.Add("A", "aA");
			_rules.Add("A", "b");
		}

		[Fact]
		public void Display() {
			StringWriter output = new();
			Console.SetOut(output);
			var states = new StateCollection(_rules);

			_test_output_helper.WriteLine(output.ToString());
			for (int i = 0; i < states.Count; i++) {
				var line = $"{i}: {states[i].ToString()}";
				_test_output_helper.WriteLine(line);
			}

			Assert.Equal(8, states.Count);
			// Check states
			Assert.Equal(">>S'->.S$|S->.AA|A->.aA|A->.b|", states[0].ToString());
			Assert.Equal("S>>S'->S.$|", states[1].ToString());
			Assert.Equal("A>>S->A.A|A->.aA|A->.b|", states[2].ToString());
			Assert.Equal("a>>A->a.A|A->.aA|A->.b|", states[3].ToString());
			Assert.Equal("b>>A->b.|", states[4].ToString());
			Assert.Equal("$>>S'->S$.|", states[5].ToString());
			Assert.Equal("A>>S->AA.|", states[6].ToString());
			Assert.Equal("A>>A->aA.|", states[7].ToString());
			// Check transitions
			var t = states.Transitions.OrderBy(x => x.from)
				.ThenBy(x => x.to)
				.ToList();
			for (int i = 0; i < t.Count; i++) {
				_test_output_helper.WriteLine($"{t[i].from} --> {t[i].to}");
			}
			Assert.Equal(11, t.Count);
			Assert.Equal((0, 1), t[0]);
			Assert.Equal((0, 2), t[1]);
			Assert.Equal((0, 3), t[2]);
			Assert.Equal((0, 4), t[3]);
			Assert.Equal((1, 5), t[4]);
			Assert.Equal((2, 3), t[5]);
			Assert.Equal((2, 4), t[6]);
			Assert.Equal((2, 6), t[7]);
			Assert.Equal((3, 3), t[8]);
			Assert.Equal((3, 4), t[9]);
			Assert.Equal((3, 7), t[10]);
		}
	}
}
