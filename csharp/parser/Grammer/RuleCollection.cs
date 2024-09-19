using System.Collections;
using System.Text;

namespace Yapp.Parser.Grammer {
	/// <summary>
	/// A collection of rules that will make up a grammer. The collection is defined by creating with a start rule
	/// and then rules are added until the grammer is complete.
	/// For rules with multiple choices you can define that rule multiple times in this collection.
	/// </summary>
	public class RuleCollection : IEnumerable<Rule> {
		private readonly List<Rule> _rules = new();

		/// <summary>
		/// Create a new rule collection by defining the start rule
		/// </summary>
		/// <param name="start_name">Name of the start rule</param>
		/// <param name="elements">Elements that make up the start rule</param>
		public RuleCollection(string start_name, string elements) {
			List<RuleItem> rules = new();
			rules.AddRange(
				elements.Select(x => char.IsUpper(x) ? new RuleItem(x.ToString()) : new RuleItem(new Token(x.ToString())))
				);
			rules.Add(new RuleItem(Token.EOT));
			_rules.Add(new Rule(start_name, rules.ToArray()));
		}

		/// <summary>
		/// Add a rule to the collection
		/// </summary>
		/// <param name="name">Name of the rule</param>
		/// <param name="elements">Elements that make up the rule</param>
		public void Add(string name, string elements) {
			Rule r = new(
				name,
				elements.Select(x => char.IsUpper(x) ? new RuleItem(x.ToString()) : new RuleItem(new Token(x.ToString()))).ToArray());
			_rules.Add(r);
		}

		/// <summary>
		/// Definition of the start rule for this collection
		/// </summary>
		public Rule StartRule => _rules[0];

		public Rule this[int idx] => _rules[idx];

		public int DisplayWidth => _rules.Max(r => r.StrLen);

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

		/// <summary>
		/// Find all rules with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal IEnumerable<Rule> Find(string name) => _rules.Where(r => r.Name == name);
	}
}
