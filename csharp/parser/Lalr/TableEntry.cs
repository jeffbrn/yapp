using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class TableEntry {
		private readonly int _idx;
		private readonly Dictionary<RuleItem, int > _gotos = new ();
		private readonly List<RuleItem> _all_trans;
		private readonly int _max_action_len;

		internal TableEntry(int idx, StateCollection states, int from, List<RuleItem> all_trans, int max_rule_len) {
			_idx = idx;
			_all_trans = all_trans;
			_max_action_len = max_rule_len > 5 ? max_rule_len : 5;
			var transitions = states.Transitions.Where(t => t.from == from).Select(t => t.to).ToList();
			if (transitions.Any()) {
				foreach (var to in transitions) {
					var state = states[to];
					_gotos[state.Transition ?? throw new ApplicationException($"Invalid transition for {from} -> {to}")] = to;
				}
			} else {
				var state = states[from] ?? throw new ApplicationException("Invalid state");
				if (state.Count != 1) throw new ApplicationException("Invalid action state");
				Action = state.GetItems().First();
			}

			_all_trans = all_trans;
		}

		internal State.RuleWalk? Action { get; }

		public bool IsShift => Action == null;

		public int NextState(RuleItem nxt) => _gotos.GetValueOrDefault(nxt, -1);

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			var fmt = "{0,2}: {1," + (-_max_action_len) + "} | ";
			sb.AppendFormat(fmt, _idx, Action?.ToString() ?? "shift");
			return sb.ToString();
		}

		#endregion
	}
}
