using System;
using NUnit.Framework;

namespace UnityModule.CommandLine {

    public class ArgumentsTest {

        [Test]
        public void ParseTest_MainOnly() {
            Arguments.GetCommandLineArguments = () => "hoge fuga ".Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(2, Arguments.GetMainArgumentList().Count);
            Assert.AreEqual("hoge", Arguments.GetMainArgumentList()[0], "hoge");
            Assert.AreEqual("fuga", Arguments.GetMainArgumentList()[1], "fuga");
        }

        [Test]
        public void ParseTest_SwitchOnly() {
            Arguments.GetCommandLineArguments = () => "-f -b --foo --bar ".Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Assert.IsTrue(Arguments.HasSwitch("f"), "-f");
            Assert.IsTrue(Arguments.HasSwitch("b"), "-b");
            Assert.IsTrue(Arguments.HasSwitch("foo"), "--foo");
            Assert.IsTrue(Arguments.HasSwitch(new [] { "b", "bar" }), "-b|--bar");
            Assert.IsTrue(Arguments.HasSwitch(new [] { "f", "faa" }), "-f|--faa");
            Assert.IsTrue(Arguments.HasSwitch(new [] { "c", "bar" }), "-c|--bar");
            Assert.IsFalse(Arguments.HasSwitch("c"), "-c");
        }

        [Test]
        public void ParseTest_OptionOnly() {
            Arguments.GetCommandLineArguments = () => "-a AAA -b BBB --ccc CCC --ddd DDD ".Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual("AAA", Arguments.GetOption("a"), "-a");
            Assert.AreEqual("CCC", Arguments.GetOption("ccc"), "--ccc");
            Assert.AreEqual("BBB", Arguments.GetOption(new [] { "b", "eee" }), "-b|--eee");
            Assert.AreEqual("DDD", Arguments.GetOption(new [] { "e", "ddd" }), "-e|--ddd");
            Assert.IsNull(Arguments.GetOption("f"), "-f");
        }

        [Test]
        public void ParseTest_Cast() {
            Arguments.GetCommandLineArguments = () => "-a 0 -b 1 --ccc true --ddd \"\" ".Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(0, Arguments.GetOptionInt("a"), "-a");
            Assert.AreEqual(true, Arguments.GetOptionBool("b"), "-b");
            Assert.AreEqual(true, Arguments.GetOptionBool("ccc"), "--ccc");
            Assert.AreEqual(0, Arguments.GetOptionInt("ddd", -1), "--ddd");
            Assert.AreEqual(false, Arguments.GetOptionBool("ddd"), "--ddd");
        }

        [Test]
        public void ParseTest_Mixed() {
            Arguments.GetCommandLineArguments = () => "hoge fuga -a 1 -b BBB -c --aaa 2 --bbb --ccc CCC ccc ".Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual(2, Arguments.GetMainArgumentList().Count);
            Assert.AreEqual("hoge", Arguments.GetMainArgumentList()[0], "hoge");
            Assert.AreEqual("fuga", Arguments.GetMainArgumentList()[1], "fuga");
            Assert.AreEqual(1, Arguments.GetOptionInt("a"), "-a");
            Assert.AreEqual("BBB", Arguments.GetOption("b"), "-b");
            Assert.AreEqual("CCC ccc", Arguments.GetOption("ccc"), "-ccc");
            Assert.IsTrue(Arguments.HasSwitch("c"), "-c");
            Assert.IsTrue(Arguments.HasSwitch(new [] { "bbb", "ccc" }), "--bbb|--ccc");
        }

    }

}