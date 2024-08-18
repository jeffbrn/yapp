using System;
using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class StateCollection {
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

		internal RuleCollection Grammer { get; }

		/// <summary>
		/// Find all states that transition from the given state
		/// </summary>
		/// <param name="idx">Index of the state to find all the outgoing transitions</param>
		/// <param name="s">The state to walk</param>
		private void FindAdjoiningStates(int idx, State s) {
			// check each rule in current state
			foreach (var walk in s.GetItems()) {
				if (walk.Current == null) continue; // if rule is already at the end then skip
				// find the rules next state
				var nxt = walk.Next();
				if (nxt == null) continue; // rule was at end
				// see if a state for that rule position already exists
				var srch = Find(nxt);
				if (srch >= 0) {
					// if so add a transition from the current state to it
					_transitions.Add((idx, srch));
					continue;
				}
				// otherwise create a new state for that rule position
				_states.Add(new State(this, walk.Current, nxt));
				// and add a transition from the current state to it
				_transitions.Add((idx, _states.Count - 1));
			}
		}

		private int Find(State.RuleWalk possible) {
			for (int i = 0; i < _states.Count; i++) {
				if (possible == _states[i].Root) return i;
			}

			return -1;
		}
	}
}
