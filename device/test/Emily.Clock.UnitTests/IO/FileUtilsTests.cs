using Emily.Clock.IO;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.IO;

[TestClass]
public class FileUtilsTests
{
    private const string Root = @"D:";

    [TestMethod]
    public void NormalizePath_should_collapse_double_backslashes()
    {
        var result = FileUtils.NormalizePath(Root, @"folder\\file.wav");
        Assert.AreEqual(@"D:\folder\file.wav", result);
    }

    [TestMethod]
    public void NormalizePath_should_convert_forward_slashes_when_path_starts_with_root()
    {
        var result = FileUtils.NormalizePath(Root, @"D:/folder/file.wav");
        Assert.AreEqual(@"D:\folder\file.wav", result);
    }

    [TestMethod]
    public void NormalizePath_should_prepend_root_for_relative_path()
    {
        var result = FileUtils.NormalizePath(Root, @"folder\file.wav");
        Assert.AreEqual(@"D:\folder\file.wav", result);
    }

    [TestMethod]
    public void NormalizePath_should_strip_colon_and_re_root()
    {
        var result = FileUtils.NormalizePath(Root, @"C:\folder\file.wav");
        Assert.AreEqual(@"D:\folder\file.wav", result);
    }

    [TestMethod]
    public void ToSizeString_should_format_bytes()
    {
        var result = FileUtils.ToSizeString(512);
        Assert.AreEqual("512.00 B", result);
    }

    [TestMethod]
    public void ToSizeString_should_format_gigabytes()
    {
        // 1536 MB = 1.5 GB — stays below int.MaxValue to avoid nanoFramework uint→int cast issue
        var result = FileUtils.ToSizeString(1536u * 1024u * 1024u);
        Assert.AreEqual("1.50 GB", result);
    }

    [TestMethod]
    public void ToSizeString_should_format_kilobytes()
    {
        var result = FileUtils.ToSizeString(2048u);
        Assert.AreEqual("2.00 KB", result);
    }

    [TestMethod]
    public void ToSizeString_should_format_megabytes()
    {
        var result = FileUtils.ToSizeString(2u * 1024u * 1024u);
        Assert.AreEqual("2.00 MB", result);
    }
}
