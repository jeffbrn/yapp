using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapp.Parser.Grammer {
	public class RuleCollection {
		private readonly List<Rule> _rules = new();

		public RuleCollection(string start_name, string elements) {
			Add(start_name, elements);
		}

		public void Add(string name, string elements) {
			Rule r = new(name, elements.Select(x => new Token(x.ToString())).ToArray());
			_rules.Add(r);
		}

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			foreach (var rule in _rules) {
				sb.AppendLine(rule.ToString());
			}

			return sb.ToString();
		}

		#endregion

		/// <summary>
		/// Walk a rule to find all other rules that are part of the incoming rule state
		/// </summary>
		/// <param name="from"></param>
		/// <returns></returns>
		internal List<Rule> Walk(Rule from) {
			List<Rule> retval = [from];
			var chk = from.Current;
			if (chk == null || chk == Token.EOT) return retval;
			foreach (var rule in Find(chk.Name)) {
				if (rule.Equals(from)) continue;
				retval.AddRange(Walk(rule));
			}

			return retval;
		}

		private IEnumerable<Rule> Find(string name) =>
			_rules.Where(r => r.Name == name);
	}
}
