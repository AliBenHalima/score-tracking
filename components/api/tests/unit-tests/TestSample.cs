using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests
{
    public class TestSample
    {
        [Theory]
        [ClassData(typeof(CalculatorTestData2))]
        public void Method_For_Testing_Data_Class(int value1, int value2, int expected)
        {
            Assert.True(true);
        }

        [Theory]
        [MemberData(nameof(CalculatorTestMemberData))]
        public void Method_For_Testing_Member_Data(int value1, int value2, int expected)
        {
            Assert.True(true);
        }

        public static IEnumerable<object[]> CalculatorTestMemberData => new List<object[]>
            {
                new object[] { 1, 2, 3 },
                new object[] { 4, 5, 6 },
                new object[] { 7, 8, 9 },
            };
    }

    public class CalculatorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { 5, 6, 7 };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CalculatorTestData2 : TheoryData<int, int, int>
    {
        public CalculatorTestData2()
        {
            Add(1, 2, 3);
            Add(8, 4, 5);
        }
    }
}
