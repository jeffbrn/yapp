using System.Text;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class State {
		internal class RuleWalk {
			private readonly Rule _rule;
			private readonly int _posn;

			public RuleWalk(Rule rule, int posn) {
				_rule = rule;
				_posn = posn;
			}

			public Rule Rule => _rule;
			public int Pos => _posn;

			public Token? Current => _posn < _rule.Length ? _rule[_posn] : null;

			public RuleWalk Next() => new RuleWalk(_rule, _posn + 1);

			#region Overrides of Object

			/// <inheritdoc />
			public override string ToString() {
				StringBuilder sb = new();
				sb.Append(_rule.Name);
				sb.Append("->");
				for (int i = 0; i < _rule.Length; i++) {
					if (i == _posn) sb.Append(".");
					sb.Append(_rule[i]);
				}
				if (_posn == _rule.Length) sb.Append(".");
				return sb.ToString();
			}

			#endregion
		}
	}
}
