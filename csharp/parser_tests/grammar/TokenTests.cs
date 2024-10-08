﻿using Yapp.Parser.Grammar;

namespace Yapp.Tests.grammar {
	public class TokenTests {
		[Fact]
		public void NameCheck() {
			Token t = new("dog");
			Assert.Equal("dog", t.Name);
		}

		[Fact]
		public void EqualityCheck() {
			Token t1 = new("Apple");
			Assert.Equal(t1, t1);
			Token t2 = new("apple");
			Assert.False(t1 == t2);
			Token t3 = new("apple");
			Assert.True(t2 == t3);
		}
	}
}
