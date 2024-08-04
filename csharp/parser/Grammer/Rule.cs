using System.Text;

namespace Yapp.Parser.Grammer {
	public class Rule(in string name, params Token[] elements) {
		private int _posn = 0;
		private readonly string _name = name;

		public string Name => _name;

		public Token this[int idx] => elements[idx];
		public int Length => elements.Length;

		#region Overrides of Object

		/// <inheritdoc />
		public override string ToString() {
			StringBuilder sb = new();
			sb.Append($"{Name} -> ");
			for (int i = 0; i < elements.Length; i++) {
				if (i == _posn) sb.Append(" . ");
				sb.Append($"{elements[i]} ");
			}
			if (_posn == elements.Length) sb.Append(".");
			return sb.ToString();
		}

		#endregion

		internal Token? Current => _posn < elements.Length ? elements[_posn] : null;
	}
}
