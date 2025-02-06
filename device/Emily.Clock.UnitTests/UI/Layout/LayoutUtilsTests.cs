using System.Drawing;
using Emily.Clock.UI.Layout;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI.Layout
{
    [TestClass]
    public class LayoutUtilsTests
    {
        [TestMethod]
        public void Align_TopLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopLeft);

            Assert.AreEqual(0, alignedRect.X);
            Assert.AreEqual(0, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_TopCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopCenter);

            Assert.AreEqual(25, alignedRect.X);
            Assert.AreEqual(0, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_TopRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.TopRight);

            Assert.AreEqual(50, alignedRect.X);
            Assert.AreEqual(0, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_MiddleLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleLeft);

            Assert.AreEqual(0, alignedRect.X);
            Assert.AreEqual(25, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_MiddleCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleCenter);

            Assert.AreEqual(25, alignedRect.X);
            Assert.AreEqual(25, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_MiddleRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.MiddleRight);

            Assert.AreEqual(50, alignedRect.X);
            Assert.AreEqual(25, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_BottomLeft()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomLeft);

            Assert.AreEqual(0, alignedRect.X);
            Assert.AreEqual(50, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_BottomCenter()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomCenter);

            Assert.AreEqual(25, alignedRect.X);
            Assert.AreEqual(50, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }

        [TestMethod]
        public void Align_BottomRight()
        {
            var alignThis = new Size(50, 50);
            var withinThis = new Rectangle(0, 0, 100, 100);
            var alignedRect = LayoutUtils.Align(alignThis, withinThis, ContentAlignment.BottomRight);

            Assert.AreEqual(50, alignedRect.X);
            Assert.AreEqual(50, alignedRect.Y);
            Assert.AreEqual(50, alignedRect.Width);
            Assert.AreEqual(50, alignedRect.Height);
        }
    }
}