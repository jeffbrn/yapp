using System.Text;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
/// <summary>
/// Defines parser state as the grammer is walked from the start Rule
/// </summary>
public class State {
	/// <summary>
	/// Defines a rule as it is being walked by the state machine
	/// </summary>
	/// <param name="rule">Rule being walked</param>
	/// <param name="posn">The current rule position for this state</param>
	internal class RuleWalk(Rule rule, int posn) : IEquatable<RuleWalk> {
		private Rule Rule { get; } = rule;

		private int Pos { get; } = posn;

		/// <summary>
		/// The current rule element in this state
		/// </summary>
		internal RuleItem? Current => Pos < Rule.Length ? Rule[Pos] : null;
		
		/// <summary>
		/// Move to the next state as the rule is walked
		/// </summary>
		/// <returns></returns>
		internal RuleWalk? Next() => Pos < Rule.Length ? new RuleWalk(Rule, Pos + 1) : null;

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			sb.Append(Rule.Name);
			sb.Append("->");
			for (int i = 0; i < Rule.Length; i++) {
				if (i == Pos) sb.Append(".");
				sb.Append(Rule[i]);
			}
			if (Pos == Rule.Length) sb.Append(".");
			return sb.ToString();
		}

		#endregion

		#region Equality members

		/// <inheritdoc />
		public bool Equals(RuleWalk? other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Rule.Equals(other.Rule) && Pos == other.Pos;
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
			return HashCode.Combine(Rule, Pos);
		}

		public static bool operator ==(RuleWalk? left, RuleWalk? right) {
			return Equals(left, right);
		}

		public static bool operator !=(RuleWalk? left, RuleWalk? right) {
			return !Equals(left, right);
		}

		#endregion
	}
	
	/// <summary>
	/// All the rules and positions for this state
	/// </summary>
	private readonly HashSet<RuleWalk> _members = new();

	/// <summary>
	/// Construct state from start rule
	/// </summary>
	internal State(StateCollection states) {
		// no transition element to the initial state
		Transition = null;
		// add all rules in their initial position
		foreach (var r in states.Grammer) {
			_members.Add(new(r, 0));
		}

		Root = new RuleWalk(states.Grammer.StartRule, 0);
	}

	/// <summary>
	/// Create a state from the given transition and rule state
	/// </summary>
	/// <param name="states">Parent collection</param>
	/// <param name="transition">Transition element to the root rule position</param>
	/// <param name="root">The root rule that defines this state</param>
	internal State(StateCollection states, RuleItem transition, RuleWalk root) {
		Root = root;
		Transition = transition;
		_members.Add(root);
		var srch = root.Current;
		if (srch == null) return;
		foreach (var r in states.Grammer.Find(srch.ToString())) {
			_members.Add(new(r, 0));
		}
	}

	/// <summary>
	/// The transition element that led to this state
	/// </summary>
	private RuleItem? Transition { get; }

	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"{Transition?.ToString() ?? string.Empty}>>");
		foreach (var r in _members) {
			sb.Append($"{r.ToString()}|");
		}

		return sb.ToString();
	}

	internal IEnumerable<RuleWalk> GetItems() => _members;

	internal RuleWalk Root { get; }
}

}
