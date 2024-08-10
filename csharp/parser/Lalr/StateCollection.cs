using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class StateCollection {
		private readonly List<State> _states = new();
		private readonly RuleCollection _rules;

		public StateCollection(RuleCollection rules) {
			_rules = rules;
		}

		/// <summary>
		/// Walk a rule to find all other rules that are part of the incoming rule state
		/// </summary>
		/// <param name="from"></param>
		/// <returns></returns>
		internal List<State.RuleWalk> Walk(Rule from) {
			List<State.RuleWalk> retval = [new State.RuleWalk(from, 0)];
			var chk = retval[0].Current;
			if (chk == null || chk.IsTerminal) return retval;
			foreach (var rule in _rules.Find(chk.Rule)) {
				if (rule.Equals(from)) continue;
				retval.AddRange(Walk(rule));
			}

			return retval;
		}
	}
}
