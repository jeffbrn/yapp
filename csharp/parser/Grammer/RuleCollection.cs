using System.Text;

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

		internal IEnumerable<Rule> Find(string name) =>
			_rules.Where(r => r.Name == name);
	}
}
