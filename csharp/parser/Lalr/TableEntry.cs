using System;
using System.Text;
using Yapp.Parser.Grammar;

namespace Yapp.Parser.Lalr {
	public class TableEntry {
		private readonly int _idx;
		private readonly Dictionary<RuleItem, int > _gotos = new ();
		private readonly List<RuleItem> _all_trans;
		private readonly int _max_action_len;

		public TableEntry(int idx, StateCollection states, List<RuleItem> all_trans, int max_rule_len) {
			_idx = idx;
			_all_trans = all_trans;
			_max_action_len = max_rule_len > 5 ? max_rule_len : 5;
			var transitions = states.Transitions.Where(t => t.from == idx)
				.Select(t => t.to)
				.ToList();
			if (transitions.Any()) {
				foreach (var to in transitions) {
					var state = states[to];
					_gotos[state.Transition ?? throw new ApplicationException($"Invalid transition for {idx} -> {to}")] = to;
				}
			} else {
				var state = states[idx] ?? throw new ApplicationException("Invalid state");
				if (state.Count != 1) throw new ApplicationException("Invalid action state");
				Action = state.GetItems().First();
			}
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
			foreach (var nxt in _all_trans.Select(NextState)) {
				sb.AppendFormat("{0} | ", nxt < 0 ? " " : nxt.ToString());
			}
			return sb.ToString();
		}

		#endregion
	}
}
