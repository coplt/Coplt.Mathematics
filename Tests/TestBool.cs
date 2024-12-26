using Coplt.Mathematics;

namespace Tests;

public class TestBool
{
    [Test]
    public static void Test1()
    {
        {
            var a = new uint4(1, 2, 3, 4);
            // ReSharper disable once EqualExpressionComparison
            #pragma warning disable CS1718 // Comparison made to same variable
            var r = (a == a).all();
            #pragma warning restore CS1718 // Comparison made to same variable
            Console.WriteLine(r);
            Assert.IsTrue(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(1, 2, 3, 0);
            var r = (a == b).all();
            Console.WriteLine(r);
            Assert.IsFalse(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(1, 0, 3, 4);
            var r = (a == b).all();
            Console.WriteLine(r);
            Assert.IsFalse(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(0, 2, 3, 4);
            var r = (a == b).all();
            Console.WriteLine(r);
            Assert.IsFalse(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(0, 0, 0, 0);
            var r = (a == b).all();
            Console.WriteLine(r);
            Assert.IsFalse(r);
        }
    }


    [Test]
    public static void Test2()
    {
        {
            var a = new uint4(1, 2, 3, 4);
            // ReSharper disable once EqualExpressionComparison
            #pragma warning disable CS1718 // Comparison made to same variable
            var r = (a == a).any();
            #pragma warning restore CS1718 // Comparison made to same variable
            Console.WriteLine(r);
            Assert.IsTrue(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(1, 2, 3, 0);
            var r = (a == b).any();
            Console.WriteLine(r);
            Assert.IsTrue(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(1, 0, 3, 4);
            var r = (a == b).any();
            Console.WriteLine(r);
            Assert.IsTrue(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(0, 2, 3, 4);
            var r = (a == b).any();
            Console.WriteLine(r);
            Assert.IsTrue(r);
        }
        {
            var a = new uint4(1, 2, 3, 4);
            var b = new uint4(0, 0, 0, 0);
            var r = (a == b).any();
            Console.WriteLine(r);
            Assert.IsFalse(r);
        }
    }
}
