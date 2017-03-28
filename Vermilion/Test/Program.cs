using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vermilion;
using Vermilion.Expressions;

namespace Test {
	class Program {
		static void Main(string[] args) {
			Expression[] Expressions = Compiler.Parse(File.ReadAllText("Test.vrm"));

			for (int i = 0; i < Expressions.Length; i++) {
				Console.WriteLine(Expressions[i]);
			}

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}