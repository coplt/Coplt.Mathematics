using System.Runtime.Intrinsics;
using Coplt.Mathematics;

namespace Tests.Cmp;

public class TestIntCmp
{
    [Test]
    public void Test1()
    {
        var a = math.isFinite(new float3(1, 2, 3));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.all(a), Is.True);
            Assert.That(math.any(a), Is.True);
            Assert.That(math.none(a), Is.False);
        }
    }
    [Test]
    public void Test2()
    {
        var a = new float3(1, 2, 3) == new float3(3, 2, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.all(a), Is.False);
            Assert.That(math.any(a), Is.True);
            Assert.That(math.none(a), Is.False);
        }
    }
    [Test]
    public void Test3()
    {
        var a = math.isNaN(new float3(1, 2, 3));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(math.all(a), Is.False);
            Assert.That(math.any(a), Is.False);
            Assert.That(math.none(a), Is.True);
        }
    }
}
