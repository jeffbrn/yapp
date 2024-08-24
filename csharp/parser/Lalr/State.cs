using System.Text;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
/// <summary>
/// Defines parser state as the grammer is walked from the start Rule
/// </summary>
public class State : IEquatable<State> {
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

		internal bool AtEnd => Pos >= Rule.Length;
		
		/// <summary>
		/// Move to the next state as the rule is walked
		/// </summary>
		/// <returns></returns>
		internal RuleWalk Next() => Pos < Rule.Length ? new RuleWalk(Rule, Pos + 1) : throw new ApplicationException("Tried to go past the end of a rule");

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
	private readonly List<RuleWalk> _members = new();

	/// <summary>
	/// The transition element that led to this state
	/// </summary>
	private readonly RuleItem? _transition;

	/// <summary>
	/// Construct state from start rule
	/// </summary>
	internal State(StateCollection states) {
		// no transition element to the initial state
		_transition = null;
		// add all rules in their initial position
		_members.AddRange(states.Grammer.Select(r => new RuleWalk(r, 0)));
	}

	/// <summary>
	/// Create a state from the given transition and rule state
	/// </summary>
	/// <param name="states">Parent collection</param>
	/// <param name="transition">Transition element to the root rule position</param>
	/// <param name="roots">The root rule that defines this state</param>
	internal State(StateCollection states, RuleItem transition, IEnumerable<RuleWalk> roots) {
		_transition = transition;
		var initial = roots.ToList();
		_members.AddRange(initial);
//		Console.WriteLine($"Creating state with transition => {transition}");
		if (transition.IsTerminal) return;
		if (initial.All(r => r.AtEnd)) return;
		// find all rules for transition to add to state
		var trans_rules = states.Grammer.Find(transition.Rule)
			.Select(r => new RuleWalk(r, 0));
//		_members.AddRange(trans_rules);
		foreach (var r in trans_rules) {
//			Console.WriteLine($"Add rule '{r}' to state: {this.ToString()}");
			_members.Add(r);
		}
	}

	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"{_transition?.ToString() ?? string.Empty}>>");
		foreach (var r in _members) {
			sb.Append($"{r.ToString()}|");
		}

		return sb.ToString();
	}

	internal IEnumerable<RuleWalk> GetItems() => _members;

//	private List<RuleWalk> Roots { get; }

	#region Equality members

	/// <inheritdoc />
	public bool Equals(State? other) {
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		if (!Equals(_transition, other._transition)) return false;
		if (_members.Count != other._members.Count) return false;
		foreach (var o in other._members) {
			if (_members.All(x => !x.Equals(o))) return false;
		}

		return true;
	}

	/// <inheritdoc />
	public override bool Equals(object? obj) {
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((State)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode() {
		return HashCode.Combine(_members, _transition);
	}

	public static bool operator ==(State? left, State? right) {
		return Equals(left, right);
	}

	public static bool operator !=(State? left, State? right) {
		return !Equals(left, right);
	}

	#endregion
}

}
