using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermilion.Expressions;

namespace Vermilion {
	public class TokenizerException : Exception {
		public TokenizerException(int Line, int Column, string Msg) :
			base(string.Format("[{0}, {1}] {2}", Line, Column, Msg)) {
		}

		public TokenizerException(int Line, int Column, string Fmt, params object[] Args) :
			base(string.Format("[{0}, {1}] {2}", Line, Column, string.Format(Fmt, Args))) {
		}
	}

	public class Compiler {
		public static Expression[] Parse(string Src) {
			List<Expression> Expressions = new List<Expression>();
			Token[] Tokens = Tokenizer.Tokenize(Src);

			return Expressions.ToArray();
		}
	}
}
