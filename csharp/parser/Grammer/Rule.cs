using System.Text;

namespace Yapp.Parser.Grammer {
	public class Rule : IEquatable<Rule> {
		private readonly string _name;
		private readonly RuleItem[] _elements;
		private readonly int _hash;

		public Rule(in string name, params RuleItem[] elements) {
			_elements = elements;
			_name = name;
			uint h = 0;
			foreach (var e in elements) {
				h ^= (uint)e.GetHashCode();
			}
			_hash = (int)h;
		}

		public string Name => _name;

		public RuleItem this[int idx] => _elements[idx];
		public int Length => _elements.Length;

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			sb.Append($"{Name} -> ");
			foreach (var t in _elements) {
				sb.Append($"{t} ");
			}
			return sb.ToString();
		}

		#endregion

		#region Equality members

		/// <inheritdoc />
		public bool Equals(Rule? other) {
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			if (_name != other._name || Length != other.Length) return false;
			for (int i = 0; i < Length; i++) {
				if (this[i] != other[i]) return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override bool Equals(object? obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Rule)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() {
			return _hash;
		}

		public static bool operator ==(Rule? left, Rule? right) {
			return Equals(left, right);
		}

		public static bool operator !=(Rule? left, Rule? right) {
			return !Equals(left, right);
		}

		#endregion
	}
}
