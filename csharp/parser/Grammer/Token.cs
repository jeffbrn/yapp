using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yapp.Parser.Grammer {
	public class Token(in string name) : IEquatable<Token> {
		private readonly string _name = name;

		public string Name => _name;

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			return Name;
		}

		#endregion

		public static readonly Token EOT = new Token("$");

		#region Equality members

		/// <inheritdoc />
		public bool Equals(Token? other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _name == other._name;
		}

		/// <inheritdoc />
		public override bool Equals(object? obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Token)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() {
			return _name.GetHashCode();
		}

		public static bool operator ==(Token? left, Token? right) {
			return Equals(left, right);
		}

		public static bool operator !=(Token? left, Token? right) {
			return !Equals(left, right);
		}

		#endregion
	}
}
