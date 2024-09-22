using System;
using System.Collections;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class StateCollection : IEnumerable<State> {
		private readonly List<State> _states = new();
		private readonly List<(int from, int to)> _transitions = new();

		public StateCollection(RuleCollection rules) {
			Grammer = rules;
			int curr_state = 0;
			// set initial state
			_states.Add(new State(this));
			// walk all states to find adjoining ones
			while (curr_state < _states.Count) {
				FindAdjoiningStates(curr_state, _states[curr_state++]);
			}
		}

		public int Count => _states.Count;
		public State this[int index] => _states[index];
		public IEnumerable<(int from, int to)> Transitions => _transitions;

		public List<RuleItem> GetTransitionList() {
			var items = _states.Where(x => x.Transition != null)
				.Select(x => x.Transition ?? throw new InvalidOperationException("Filter didn't work"))
				.Distinct()
				.OrderBy(r => {
					if (r.IsTerminal) {
						return r.Token == Token.EOT ? -1 : -2;
					}

					return r.Rule.Length == 1 ? 1 : 2;
				});
			return items.ToList();
		}

		public RuleCollection Grammer { get; }

		/// <summary>
		/// Find all states that transition from the given state
		/// </summary>
		/// <param name="idx">Index of the state to find all the outgoing transitions</param>
		/// <param name="s">The state to walk</param>
		private void FindAdjoiningStates(int idx, State s) {
//			Console.WriteLine($"* FindAdjoiningState idx = {idx}: {s.ToString()}");
			// check each rule in current state
			foreach (var walk in s.GetItems().GroupBy(r => r.Current)) {
				if (walk.Key == null) continue; // if rule is already at the end then skip
//				Console.WriteLine($"    Walking rule: {walk.Key}");
				var tmp = walk.ToList();
				// find the rules next state
				var nxt = new State(this, walk.Key, walk.Select(r => r.Next()));
//				Console.WriteLine($"    Candidate new state: {nxt}");
				// see if a state for that rule position already exists
				var srch = Find(nxt);
				if (srch >= 0) {
//					Console.WriteLine($"    found existing @ idx = {srch}");
					// if so add a transition from the current state to it
					_transitions.Add((idx, srch));
					continue;
				}
//				Console.WriteLine($"    adding state @ idx = {_states.Count}");
				// otherwise create a new state for that rule position
				_states.Add(nxt);
				// and add a transition from the current state to it
				_transitions.Add((idx, _states.Count - 1));
			}
		}

		private int Find(State possible) {
			for (int i = 0; i < _states.Count; i++) {
				if (possible == _states[i]) return i;
			}

			return -1;
		}

		#region Implementation of IEnumerable

		/// <inheritdoc />
		public IEnumerator<State> GetEnumerator() {
			return _states.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		#endregion
	}
}
