using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr
{
	public class ParseTable {
		private readonly StateCollection _states;
		private readonly int _max_rule_len;
		private readonly List<RuleItem> _cols;
		private readonly List<TableEntry> _entries = new();

		public ParseTable(StateCollection states) {
			_states = states;
			_max_rule_len = _states.Grammer.DisplayWidth;
			_cols = _states.Where(s => s.Transition != null)
				.Select(s => s.Transition)
				.Distinct()
				.OrderBy(t => t!.IsTerminal
					? (t.Token == Token.EOT ? -1 : -5)
					: t.Rule.Length
				).ToList()!;
			for (int i = 0; i < _states.Count; i++) {
				_entries.Add(new TableEntry(i,_states,_cols,_max_rule_len));
			}
		}

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			var fmt = "    {0," + (-_max_rule_len) + "} | ";
			sb.AppendFormat(fmt, "ACTION");
			foreach (var c in _cols)
			{
				sb.Append($"{c} | ");
			}
			sb.AppendLine();
			sb.AppendFormat("===={0}===", new string('=', _max_rule_len));
			foreach (var c in _cols)
			{
				sb.AppendFormat("{0}===", new string('=', c.ToString().Length));
			}
			sb.AppendLine();
			for (int i = 0; i < _entries.Count; i++) {
				sb.AppendLine(_entries[i].ToString());
			}
			return sb.ToString();
		}

		#endregion
	}
}
