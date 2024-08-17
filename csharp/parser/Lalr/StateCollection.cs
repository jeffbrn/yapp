using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yapp.Parser.Grammer;

namespace Yapp.Parser.Lalr {
	public class StateCollection {
		private readonly List<State> _states = new();
		private readonly List<(int from, int to)> _transitions = new();

		public StateCollection(RuleCollection rules) {
			Grammer = rules;
			int curr_state = 0;
			_states.Add(new State(this));
			while (curr_state < _states.Count) {
				FindAdjoiningStates(curr_state, _states[curr_state++]);
			}
		}

		public int Count => _states.Count;
		public State this[int index] => _states[index];
		public IEnumerable<(int from, int to)> Transitions => _transitions;

		internal RuleCollection Grammer { get; }

		private void FindAdjoiningStates(int idx, State s) {
			foreach (var walk in s.GetItems()) {
				if (walk.Current == null) continue;
				var nxt = walk.Next();
				if (nxt == null) continue;
				var srch = Find(nxt);
				if (srch >= 0) {
					_transitions.Add((idx, srch));
					continue;
				}
				_states.Add(new State(this, walk.Current.ToString(), nxt));
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
