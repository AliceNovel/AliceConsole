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
    public void TestConversation()
    {
        string input = "[Hello World!]";
        Assert.AreEqual(" \"Hello World!\"", Anov.Read(input));
    }

    [TestMethod]
    public void TestPlace()
    {
        string input = "> Neo city";
        Assert.AreEqual("/ Neo city /", Anov.Read(input));
    }
}
