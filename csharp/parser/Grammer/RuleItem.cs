namespace Yapp.Parser.Grammer {
	public class RuleItem {
		private readonly Token? _token = null;
		private readonly string? _rule = null;

		public RuleItem(Token t) {
			_token = t;
		}

		public RuleItem(string rule) {
			_rule = rule;
		}

		public Token Token => _token ?? throw new InvalidOperationException("Tried to access token on rule item");

		public string Rule => _rule ?? throw new InvalidOperationException("Tried to access rule on token item");

		public bool IsTerminal => _rule == null;

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			return IsTerminal ? Token.ToString() : Rule;
		}

		#endregion
	}
}
