using System;
using SBLScripting;
using Xunit;

namespace SBScriptingTests
{
    public class ParseTests
    {
        [Fact]
        public void ParseTest1()
        {
            //
            var src = "@module test";
            //
            var compiler = new SBLCompiler();
            var result =  compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }

        [Fact]
        public void ParseTest2()
        {
            //
            var src = "@import ('std')";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }

        [Fact]
        public void ParseTest3()
        {
            //
            var src = "@import ('std') as XYZ\n" +
                            "var _decl = 10;\n" +
                            "var declx = new x.y.z();";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }


        [Fact]
        public void ParseTest4()
        {
            //
            var src = "var a = 10; a = 20;";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }

        [Fact]
        public void ParseTest5()
        {
            //
            var src = "b = b + 20;";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }

        [Fact]
        public void ParseTest6()
        {
            //
            var src = "for (var x = 10; x < x; x = x + 1) {}";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }


        [Fact]
        public void ParseTest7()
        {
            //
            var src = "for (var x = 10; x < x; x = x + 1) { var z = 10; }";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }

        [Fact]
        public void ParseTest8()
        {
            //
            var src = "foreach (var d in k) {var xy = 12;}";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }


        [Fact]
        public void ParseTest9()
        {
            //
            var src = "while (d < 10) {var xy = 12;}";
            //
            var compiler = new SBLCompiler();
            var result = compiler.Parse(src);
            //
            Xunit.Assert.True(result);
        }
    }
}
