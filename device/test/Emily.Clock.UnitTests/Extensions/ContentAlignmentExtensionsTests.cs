using System;
using System.Drawing;
using nanoFramework.Presentation;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.Extensions;

[TestClass]
public class ContentAlignmentExtensionsTests
{
    [TestMethod]
    public void GetHorizontalAlignment_should_return_Center_for_BottomCenter()
    {
        Assert.AreEqual(HorizontalAlignment.Center, ContentAlignment.BottomCenter.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Center_for_MiddleCenter()
    {
        Assert.AreEqual(HorizontalAlignment.Center, ContentAlignment.MiddleCenter.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Center_for_TopCenter()
    {
        Assert.AreEqual(HorizontalAlignment.Center, ContentAlignment.TopCenter.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Left_for_BottomLeft()
    {
        Assert.AreEqual(HorizontalAlignment.Left, ContentAlignment.BottomLeft.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Left_for_MiddleLeft()
    {
        Assert.AreEqual(HorizontalAlignment.Left, ContentAlignment.MiddleLeft.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Left_for_TopLeft()
    {
        Assert.AreEqual(HorizontalAlignment.Left, ContentAlignment.TopLeft.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Right_for_BottomRight()
    {
        Assert.AreEqual(HorizontalAlignment.Right, ContentAlignment.BottomRight.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Right_for_MiddleRight()
    {
        Assert.AreEqual(HorizontalAlignment.Right, ContentAlignment.MiddleRight.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetHorizontalAlignment_should_return_Right_for_TopRight()
    {
        Assert.AreEqual(HorizontalAlignment.Right, ContentAlignment.TopRight.GetHorizontalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Bottom_for_BottomCenter()
    {
        Assert.AreEqual(VerticalAlignment.Bottom, ContentAlignment.BottomCenter.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Bottom_for_BottomLeft()
    {
        Assert.AreEqual(VerticalAlignment.Bottom, ContentAlignment.BottomLeft.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Bottom_for_BottomRight()
    {
        Assert.AreEqual(VerticalAlignment.Bottom, ContentAlignment.BottomRight.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Center_for_MiddleCenter()
    {
        Assert.AreEqual(VerticalAlignment.Center, ContentAlignment.MiddleCenter.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Center_for_MiddleLeft()
    {
        Assert.AreEqual(VerticalAlignment.Center, ContentAlignment.MiddleLeft.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Center_for_MiddleRight()
    {
        Assert.AreEqual(VerticalAlignment.Center, ContentAlignment.MiddleRight.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Top_for_TopCenter()
    {
        Assert.AreEqual(VerticalAlignment.Top, ContentAlignment.TopCenter.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Top_for_TopLeft()
    {
        Assert.AreEqual(VerticalAlignment.Top, ContentAlignment.TopLeft.GetVerticalAlignment());
    }

    [TestMethod]
    public void GetVerticalAlignment_should_return_Top_for_TopRight()
    {
        Assert.AreEqual(VerticalAlignment.Top, ContentAlignment.TopRight.GetVerticalAlignment());
    }
}
