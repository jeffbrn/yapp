using System.Text;
using Xunit.Abstractions;

using Yapp.Parser.Grammar;
using Yapp.Parser.Lalr;

namespace Yapp.Tests.lalr {
	public class StateCollectionTests
    {
        private readonly ITestOutputHelper _test_output_helper;
        private readonly RuleCollection _rules1 = TestGrammars.Grammar1();
		private readonly RuleCollection _rules2 = TestGrammars.Grammar2();
        private readonly StringWriter _output = new();

		public StateCollectionTests(ITestOutputHelper testOutputHelper)
        {
            _test_output_helper = testOutputHelper;
            // setup to capture console output
            Console.SetOut(_output);
		}

        private void DumpStates(StateCollection states, List<(int from, int to)> transitions) {
			// dump any captured console output
			_test_output_helper.WriteLine(_output.ToString());
			// output the discovered states
			for (int i = 0; i < states.Count; i++) {
				var line = $"{i}: {states[i]}";
				_test_output_helper.WriteLine(line);
			}
			for (int i = 0; i < transitions.Count; i++) {
				_test_output_helper.WriteLine($"{transitions[i].from} --> {transitions[i].to}");
			}
		}

		private static string TDefs(StateCollection states, int idx) {
			var list = states.Transitions.Where(t => t.from == idx)
				.OrderBy(t => t.to)
				.Select(t => t.to.ToString());
			StringBuilder sb = new();
			foreach (var t in list) {
				sb.Append($"{t}|");
			}
			return sb.ToString();
		}


		[Fact]
        public void WalkRules1()
        {
            var states = new StateCollection(_rules1);
			var t = states.Transitions.OrderBy(x => x.from)
				.ThenBy(x => x.to)
				.ToList();
			DumpStates(states, t);

            Assert.Equal(8, states.Count);
            // Check states
            Assert.Equal(">>S'->.S$|S->.AA|A->.aA|A->.b|", states[0].ToString());
            Assert.Equal("S>>S'->S.$|S->.AA|", states[1].ToString());
            Assert.Equal("A>>S->A.A|A->.aA|A->.b|", states[2].ToString());
            Assert.Equal("a>>A->a.A|", states[3].ToString());
            Assert.Equal("b>>A->b.|", states[4].ToString());
            Assert.Equal("$>>S'->S$.|", states[5].ToString());
            Assert.Equal("A>>S->AA.|", states[6].ToString());
            Assert.Equal("A>>A->aA.|", states[7].ToString());
            // Check transitions
            Assert.Equal(10, t.Count);
			Assert.Equal("1|2|3|4|", TDefs(states, 0));
			Assert.Equal("2|5|", TDefs(states, 1));
			Assert.Equal("3|4|6|", TDefs(states, 2));
			Assert.Equal("7|", TDefs(states, 3));
			Assert.Equal("", TDefs(states, 4));
			Assert.Equal("", TDefs(states, 5));
			Assert.Equal("", TDefs(states, 6));
			Assert.Equal("", TDefs(states, 7));
        }

		[Fact]
        public void WalkRules2() {
            var states = new StateCollection(_rules2);
            var t = states.Transitions.OrderBy(x => x.from)
                .ThenBy(x => x.to)
                .ToList();
            DumpStates(states, t);

			// Check states
			Assert.Equal(11, states.Count);
			Assert.Equal(">>S'->.E$|E->.E-T|E->.T|T->.n|T->.(E)|", states[0].ToString());
			Assert.Equal("E>>S'->E.$|E->E.-T|E->.E-T|E->.T|", states[1].ToString());
			Assert.Equal("T>>E->T.|", states[2].ToString());
			Assert.Equal("n>>T->n.|", states[3].ToString());
			Assert.Equal("(>>T->(.E)|", states[4].ToString());
			Assert.Equal("$>>S'->E$.|", states[5].ToString());
			Assert.Equal("->>E->E-.T|", states[6].ToString());
			Assert.Equal("E>>E->E.-T|E->.E-T|E->.T|", states[7].ToString());
			Assert.Equal("E>>T->(E.)|E->.E-T|E->.T|", states[8].ToString());
			Assert.Equal("T>>E->E-T.|", states[9].ToString());
			Assert.Equal(")>>T->(E).|", states[10].ToString());
			// Check state transitions
            Assert.Equal(16, t.Count);
			Assert.Equal("1|2|3|4|", TDefs(states, 0));
			Assert.Equal("2|5|6|7|", TDefs(states, 1));
			Assert.Equal("", TDefs(states, 2));
			Assert.Equal("", TDefs(states, 3));
			Assert.Equal("8|", TDefs(states, 4));
			Assert.Equal("", TDefs(states, 5));
			Assert.Equal("9|", TDefs(states, 6));
			Assert.Equal("2|6|7|", TDefs(states, 7));
			Assert.Equal("2|7|10|", TDefs(states, 8));
			Assert.Equal("", TDefs(states, 9));
			Assert.Equal("", TDefs(states, 10));
		}

	}
}
