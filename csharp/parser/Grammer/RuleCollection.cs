using System.Collections;
using System.Text;

namespace Yapp.Parser.Grammer {
	public class RuleCollection : IEnumerable<Rule> {
		private readonly List<Rule> _rules = new();

		public RuleCollection(string start_name, string elements) {
			List<RuleItem> rules = new();
			rules.AddRange(
				elements.Select(x => char.IsUpper(x) ? new RuleItem(x.ToString()) : new RuleItem(new Token(x.ToString())))
				);
			rules.Add(new RuleItem(Token.EOT));
			_rules.Add(new Rule(start_name, rules.ToArray()));
		}

		public void Add(string name, string elements) {
			Rule r = new(
				name,
				elements.Select(x => char.IsUpper(x) ? new RuleItem(x.ToString()) : new RuleItem(new Token(x.ToString()))).ToArray());
			_rules.Add(r);
		}

		public Rule StartRule => _rules[0];

		#region Overrides of Object

		/// <inheritdoc />
		public IEnumerator<Rule> GetEnumerator() {
			return _rules.GetEnumerator();
		}

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			foreach (var rule in _rules) {
				sb.AppendLine(rule.ToString());
			}

			return sb.ToString();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion

		internal IEnumerable<Rule> Find(string name) => _rules.Where(r => r.Name == name);
	}
}
