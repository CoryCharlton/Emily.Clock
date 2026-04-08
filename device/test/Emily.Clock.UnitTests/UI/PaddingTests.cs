using System.Drawing;
using Emily.Clock.UI;
using nanoFramework.TestFramework;

namespace Emily.Clock.UnitTests.UI;

[TestClass]
public class PaddingTests
{
    [TestMethod]
    public void Empty_should_have_all_zero_sides()
    {
        Assert.AreEqual((ushort)0, Padding.Empty.Top);
        Assert.AreEqual((ushort)0, Padding.Empty.Right);
        Assert.AreEqual((ushort)0, Padding.Empty.Bottom);
        Assert.AreEqual((ushort)0, Padding.Empty.Left);
    }

    [TestMethod]
    public void Equals_should_return_false_for_different_padding()
    {
        var p1 = new Padding(1);
        var p2 = new Padding(2);

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod]
    public void Equals_should_return_false_for_non_padding()
    {
        var p = new Padding(1);
        Assert.IsFalse(p.Equals("not a padding"));
    }

    [TestMethod]
    public void Equals_should_return_true_for_same_values()
    {
        var p1 = new Padding(1, 2, 3, 4);
        var p2 = new Padding(1, 2, 3, 4);

        Assert.IsTrue(p1.Equals(p2));
    }

    [TestMethod]
    public void EqualityOperator_should_return_false_for_different_values()
    {
        var p1 = new Padding(1);
        var p2 = new Padding(2);

        Assert.IsFalse(p1 == p2);
    }

    [TestMethod]
    public void EqualityOperator_should_return_true_for_same_values()
    {
        var p1 = new Padding(5);
        var p2 = new Padding(5);

        Assert.IsTrue(p1 == p2);
    }

    [TestMethod]
    public void FourValueConstructor_should_set_each_side_independently()
    {
        var p = new Padding(1, 2, 3, 4);

        Assert.AreEqual((ushort)1, p.Top);
        Assert.AreEqual((ushort)2, p.Right);
        Assert.AreEqual((ushort)3, p.Bottom);
        Assert.AreEqual((ushort)4, p.Left);
    }

    [TestMethod]
    public void GetHashCode_should_be_consistent_for_equal_values()
    {
        var p1 = new Padding(1, 2, 3, 4);
        var p2 = new Padding(1, 2, 3, 4);

        Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod]
    public void Horizontal_should_be_sum_of_left_and_right()
    {
        var p = new Padding(1, 2, 3, 4);

        Assert.AreEqual(6, p.Horizontal);
    }

    [TestMethod]
    public void InequalityOperator_should_return_true_for_different_values()
    {
        var p1 = new Padding(1);
        var p2 = new Padding(2);

        Assert.IsTrue(p1 != p2);
    }

    [TestMethod]
    public void SingleValueConstructor_should_set_all_sides_equal()
    {
        var p = new Padding(5);

        Assert.AreEqual((ushort)5, p.Top);
        Assert.AreEqual((ushort)5, p.Right);
        Assert.AreEqual((ushort)5, p.Bottom);
        Assert.AreEqual((ushort)5, p.Left);
    }

    [TestMethod]
    public void Size_should_equal_horizontal_and_vertical()
    {
        var p = new Padding(1, 2, 3, 4);
        var expected = new Size(p.Horizontal, p.Vertical);

        Assert.AreEqual(expected, p.Size);
    }

    [TestMethod]
    public void TwoValueConstructor_should_set_vertical_and_horizontal()
    {
        var p = new Padding(3, 7);

        Assert.AreEqual((ushort)3, p.Top);
        Assert.AreEqual((ushort)7, p.Right);
        Assert.AreEqual((ushort)3, p.Bottom);
        Assert.AreEqual((ushort)7, p.Left);
    }

    [TestMethod]
    public void Vertical_should_be_sum_of_top_and_bottom()
    {
        var p = new Padding(1, 2, 3, 4);

        Assert.AreEqual(4, p.Vertical);
    }
}
