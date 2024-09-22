namespace Yapp.Parser.Grammar {
	/// <summary>
	/// Defines a rule element which can be either a terminal or non-terminal
	/// </summary>
	public class RuleItem : IEquatable<RuleItem> {
		private readonly Token? _token;
		private readonly string? _rule;

		/// <summary>
		/// Defines a terminal rule element
		/// </summary>
		/// <param name="t">The token that defines the terminal value</param>
		public RuleItem(Token t) {
			_token = t;
		}

		/// <summary>
		/// Defines a non-terminal rule element
		/// </summary>
		/// <param name="rule">name of the rule</param>
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

		#region Equality members

		/// <inheritdoc />
		public bool Equals(RuleItem? other) {
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(_token, other._token) && _rule == other._rule;
		}

		/// <inheritdoc />
		public override bool Equals(object? obj) {
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((RuleItem)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() {
			return HashCode.Combine(_token, _rule);
		}

		public static bool operator ==(RuleItem? left, RuleItem? right) {
			return Equals(left, right);
		}

		public static bool operator !=(RuleItem? left, RuleItem? right) {
			return !Equals(left, right);
		}

		#endregion
	}
}
