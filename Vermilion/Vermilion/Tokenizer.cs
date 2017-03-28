using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermilion {
	[Flags]
	public enum CharType {
		Null = 1 << 0,
		Whitespace = 1 << 1,
		Newline = 1 << 2,
		Symbol = 1 << 3,
		Letter = 1 << 4,
		Digit = 1 << 5,

		LetterOrNumber = Letter | Digit
	}

	public enum TokenType {
		StringLiteral,
		Whitespace,
		Identifier,
		Symbol,
	}

	public class Token {
		public TokenType TokenType;
		public int Line, Column;
		public StringBuilder TokenSource;

		public Token(int Line, int Column, TokenType Type) {
			this.Line = Line;
			this.Column = Column;
			TokenType = Type;
			TokenSource = new StringBuilder();
		}

		public override bool Equals(object Obj) {
			if (Obj is TokenType)
				return TokenType == (TokenType)Obj;

			return base.Equals(Obj);
		}

		public override string ToString() {
			return "[" + Line + ", " + Column + "] " + TokenType + " '" + TokenSource + "'";
		}
	}

	public class Tokenizer {
		char[] SrcArray;
		int NextIndex, Line, Column;

		public Tokenizer(string Src) {
			SrcArray = Src.ToArray();
			NextIndex = 0;
			Line = 1;
			Column = 1;
		}

		public char Peek(int Advance = 0) {
			if (NextIndex + Advance >= SrcArray.Length)
				return (char)0;
			return SrcArray[NextIndex + Advance];
		}

		public char Read() {
			char C = SrcArray[NextIndex++];

			if (C == '\n') {
				Line++;
				Column = 1;
			} else
				Column++;
			return C;
		}

		public string Read(int Cnt) {
			if (Cnt > 0)
				return Read().ToString() + Read(Cnt - 1);
			return "";
		}

		public bool Expect(char C, bool Throw = false) {
			if (Peek() != C) {
				if (Throw)
					throw new TokenizerException(Line, Column + 1, "Expected '{0}', got '{1}'", C, Peek());
				return false;
			}
			return true;
		}

		public Token ParseStringLiteral() {
			if (!Expect('"'))
				return null;

			Token T = new Token(Line, Column, TokenType.StringLiteral);
			Read();

			while (Peek() != '"') {
				if (Peek() == '\\') {
					bool Valid = false;

					switch (Peek(1)) {
						case '\\':
							Valid = true;
							T.TokenSource.Append('\\');
							break;
						case '"':
							Valid = true;
							T.TokenSource.Append('"');
							break;
						default:
							throw new TokenizerException(Line, Column + 1, "Unknown escape sequence '\\{0}'", Peek(1));
					}

					if (Valid) Read(2);
				} else
					T.TokenSource.Append(Read());
			}

			Expect('"', true);
			Read();
			return T;
		}

		public Token TokenizeWhitespace() {
			if (!CharIsType(Peek(), CharType.Whitespace | CharType.Newline))
				return null;
			Token T = new Token(Line, Column, TokenType.Whitespace);

			while (CharIsType(Peek(), CharType.Whitespace | CharType.Newline))
				T.TokenSource.Append(Read());

			return T;
		}

		public Token TokenizeIdentifier() {
			if (!(CharIsType(Peek(), CharType.Letter) || Expect('_')))
				return null;
			Token T = new Token(Line, Column, TokenType.Identifier);

			while (CharIsType(Peek(), CharType.LetterOrNumber))
				T.TokenSource.Append(Read());

			return T;
		}

		public Token TokenizeSymbol() {
			if (!CharIsType(Peek(), CharType.Symbol))
				return null;
			Token T = new Token(Line, Column, TokenType.Symbol);
			T.TokenSource.Append(Read());
			return T;
		}

		public Token TokenizeAny() {
			Token T;
			if (NextIndex >= SrcArray.Length)
				return null;

			if ((T = TokenizeWhitespace()) != null)
				return T;
			if ((T = ParseStringLiteral()) != null)
				return T;
			if ((T = TokenizeSymbol()) != null)
				return T;
			if ((T = TokenizeIdentifier()) != null)
				return T;

			throw new TokenizerException(Line, Column, "Unknown token type for symbol '{0}'", Peek());
		}

		public Token[] ToTokenArray() {
			List<Token> Tokens = new List<Token>();

			Token T = null;
			while ((T = TokenizeAny()) != null)
				Tokens.Add(T);

			return Tokens.ToArray();
		}

		public static CharType GetCharType(char C) {
			if (C == (char)0)
				return CharType.Null;

			if (char.IsSymbol(C) || char.IsPunctuation(C))
				return CharType.Symbol;
			if (C == '\n')
				return CharType.Newline;
			if (char.IsWhiteSpace(C))
				return CharType.Whitespace;
			if (char.IsLetter(C))
				return CharType.Letter;
			if (char.IsDigit(C))
				return CharType.Digit;
			return CharType.Null;
		}

		public static bool CharIsType(char C, CharType T) {
			CharType CT = GetCharType(C);
			return CT == T || T.HasFlag(CT);
		}

		public static Token[] Tokenize(string Src) {
			Tokenizer T = new Tokenizer(Src);
			return T.ToTokenArray();
		}
	}
}
