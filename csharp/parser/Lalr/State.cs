using System.Text;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
public class State {
	internal class RuleWalk : IEquatable<RuleWalk> {
		private readonly Rule _rule;
		private readonly int _posn;

		public RuleWalk(Rule rule, int posn) {
			_rule = rule;
			_posn = posn;
		}

		public Rule Rule => _rule;
		public int Pos => _posn;

		public RuleItem? Current => _posn < _rule.Length ? _rule[_posn] : null;

		public RuleWalk? Next() => _posn < _rule.Length ? new RuleWalk(_rule, _posn + 1) : null;

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

		#region Equality members

		/// <inheritdoc />
		public bool Equals(RuleWalk? other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return _rule.Equals(other._rule) && _posn == other._posn;
		}

		/// <inheritdoc />
		public override bool Equals(object? obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((RuleWalk)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode() {
			return HashCode.Combine(_rule, _posn);
		}

		public static bool operator ==(RuleWalk? left, RuleWalk? right) {
			return Equals(left, right);
		}

		public static bool operator !=(RuleWalk? left, RuleWalk? right) {
			return !Equals(left, right);
		}

		#endregion
	}

	private readonly HashSet<RuleWalk> _members = new();
	private readonly StateCollection _states;

	internal State(StateCollection states) {
		_states = states;
		Transition = null;
		foreach (var r in _states.Grammer) {
			_members.Add(new(r, 0));
		}

		Root = new RuleWalk(states.Grammer.StartRule, 0);
	}

	internal State(StateCollection states, string transition, RuleWalk root) {
		_states = states;
		Root = root;
		Transition = transition;
		_members.Add(root);
		var srch = root.Current;
		if (srch == null) return;
		foreach (var r in _states.Grammer.Find(srch.ToString())) {
			_members.Add(new(r, 0));
		}
	}

	public string? Transition { get; }

	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"{Transition ?? string.Empty}>>");
		foreach (var r in _members) {
			sb.Append($"{r.ToString()}|");
		}

		return sb.ToString();
	}

	internal IEnumerable<RuleWalk> GetItems() => _members;

	internal RuleWalk Root { get; }
}

}
