using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

using Yapp.Parser.Grammer;
using Yapp.Parser.Lalr;

namespace Yapp.Tests.lalr {
	public class StateTableTests {
		private readonly ITestOutputHelper _test_output_helper;
		private readonly StateCollection _rules1;
		private readonly int _display_width1;

		public StateTableTests(ITestOutputHelper testOutputHelper) {
			_test_output_helper = testOutputHelper;

			RuleCollection rules1 = new("S'", "S");
			rules1.Add("S", "AA");
			rules1.Add("A", "aA");
			rules1.Add("A", "b");
			_rules1 = new(rules1);
			_display_width1 = rules1.DisplayWidth;
		}

		[Fact]
		public void TableEntry() {

		}
	}
}
