﻿using Coplt.Mathematics;

namespace MathTests;

public class TestBool
{
    [Test]
    public static void Test1()
    {
        {
            var a = new uint4(1, 2, 3, 4);
            // ReSharper disable once EqualExpressionComparison
            var r = (a == a).all();
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
            var r = (a == a).any();
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