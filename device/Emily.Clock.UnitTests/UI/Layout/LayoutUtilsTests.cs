using System.Drawing;
using Emily.Clock.UI;
using Emily.Clock.UI.Layout;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Layout
{
    [TestClass]
    public class LayoutUtilsTests
    {
        [TestMethod]
        public void Align_BottomCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomCenter);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(50, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_BottomCenter_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomCenter, padding);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(40, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_BottomLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomLeft);

            Assert.AreEqual(0, sut.X);
            Assert.AreEqual(50, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_BottomLeft_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomLeft, padding);

            Assert.AreEqual(10, sut.X);
            Assert.AreEqual(40, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_BottomRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomRight);

            Assert.AreEqual(50, sut.X);
            Assert.AreEqual(50, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_BottomRight_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomRight, padding);

            Assert.AreEqual(40, sut.X);
            Assert.AreEqual(40, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleCenter);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleCenter_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleCenter, padding);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleLeft);

            Assert.AreEqual(0, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleLeft_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleLeft, padding);

            Assert.AreEqual(10, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleRight);

            Assert.AreEqual(50, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_MiddleRight_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleRight, padding);

            Assert.AreEqual(40, sut.X);
            Assert.AreEqual(25, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopCenter);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(0, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopCenter_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopCenter, padding);

            Assert.AreEqual(25, sut.X);
            Assert.AreEqual(10, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopLeft);

            Assert.AreEqual(0, sut.X);
            Assert.AreEqual(0, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopLeft_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopLeft, padding);

            Assert.AreEqual(10, sut.X);
            Assert.AreEqual(10, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopRight);

            Assert.AreEqual(50, sut.X);
            Assert.AreEqual(0, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }

        [TestMethod]
        public void Align_TopRight_with_Padding()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var padding = new Padding(10);

            var sut = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopRight, padding);

            Assert.AreEqual(40, sut.X);
            Assert.AreEqual(10, sut.Y);
            Assert.AreEqual(50, sut.Width);
            Assert.AreEqual(50, sut.Height);
        }
    }
}