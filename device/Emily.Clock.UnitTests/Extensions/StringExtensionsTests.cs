using nanoFramework.TestFramework;
using System;

// ReSharper disable StringLiteralTypo
namespace Emily.Clock.UnitTests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
    public void Replace_char_replaces_correctly()
    {
        const string original = "hello world";
        const char oldChar = 'o';
        const char newChar = 'a';
        const string expected = "hella warld";

        var result = original.Replace(oldChar, newChar);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Replace_string_replaces_correctly()
    {
        const string original = "hello world";
        const string oldValue = "world";
        const string newValue = "nanoFramework";
        const string expected = "hello nanoFramework";

        var result = original.Replace(oldValue, newValue);

        Assert.AreEqual(expected, result);
    }
}