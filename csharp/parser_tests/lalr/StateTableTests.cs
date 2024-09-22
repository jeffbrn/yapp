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
			List<RuleItem> cols = _rules1.Where(s => s.Transition != null)
				.Select(s => s.Transition)
				.Distinct()
				.OrderBy(t => t!.IsTerminal
					? (t.Token == Token.EOT ? -1 : -5)
					: t.Rule.Length
				).ToList()!;
			_test_output_helper.WriteLine("Columns:-");
			StringBuilder sb = new();
			var fmt = "    {0," + (-_display_width1) + "} | ";
			sb.AppendFormat(fmt, "ACTION");
			foreach (var c in cols) {
				sb.Append($"{c} | ");
			}
			_test_output_helper.WriteLine(sb.ToString());

 			TableEntry entry = new(0, _rules1, cols, _display_width1);
			Assert.True(entry.IsShift);
			Assert.Equal(-1, entry.NextState(cols[2]));
			Assert.Equal(3, entry.NextState(cols[0]));
			_test_output_helper.WriteLine(entry.ToString());
		}

		[Fact]
		public void ParseTable() {
			var pt = new ParseTable(_rules1);
			_test_output_helper.WriteLine(pt.ToString());
		}

	}
}
