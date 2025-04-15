using AnovSyntax;

namespace Test;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void TestPeople()
    {
        string input = "- Alice";
        Assert.AreEqual("Alice", Anov.Read(input));
    }

    [TestMethod]
    public void TestFeeling()
    {
        string input = "/ happy";
        Assert.AreEqual(" (happy)", Anov.Read(input));
    }

    [TestMethod]
    public void TestPeopleAndFeeling()
    {
        string input = "- Alice / happy";
        Assert.AreEqual("Alice (happy)", Anov.Read(input));
    }

    [TestMethod]
    public void TestConversation()
    {
        string input = "[Hello World!]";
        Assert.AreEqual("\"Hello World!\"", Anov.Read(input));
    }

    [TestMethod]
    public void TestConversationCJK()
    {
        string input = "[Hello World!]";
        Assert.AreEqual("「Hello World!」", Anov.Read(input, QuoteType.CJKSingleQuote));
    }

    [TestMethod]
    public void TestPlace()
    {
        string input = "> Neo city";
        Assert.AreEqual("/ Neo city /", Anov.Read(input));
    }
}
