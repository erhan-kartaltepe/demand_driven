using System;
using DemandDriven.Pocos;
using Xunit;

namespace DemandDriven.Tests
{
    public class CsvTest
    {
        [Fact]
        public void TestEntry()
        {
            Entry ee = new Entry {ParentName = "ABC", Quantity = 1};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.CHILD_REQUIRED_ERROR);

            ee = new Entry {ChildName = "ABC", Quantity = 1};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.PARENT_REQUIRED_ERROR);

            ee = new Entry {ChildName = "ABC", ParentName = "ABC", Quantity = 0};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.QUANTITY_RANGE_ERROR);

            ee = new Entry {ChildName = "ABC", ParentName = "ABC", Quantity = 11};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.QUANTITY_RANGE_ERROR);

            ee = new Entry {ChildName = "ABC", ParentName = "ABCD", Quantity = 5};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.PARENT_REGEX_ERROR);

            ee = new Entry {ChildName = "AB", ParentName = "ABC", Quantity = 5};
            Assert.True(ee.Validate().Count == 1);
            Assert.Equal(ee.Validate()[0].ErrorMessage, Entry.CHILD_REGEX_ERROR);

            ee = new Entry {ChildName = "ABC", ParentName = "DEF", Quantity = 2};
            Assert.True(ee.Validate().Count == 0);
        }

        [Fact]
        public void TestConverter()
        {
            Exception exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, string.Empty));
            Assert.Equal(CsvConverter.INVALID_CSV_ERROR, exception.Message);

            exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, String.Empty));
            Assert.Equal(CsvConverter.INVALID_CSV_ERROR, exception.Message);

            string csv = "P,C,Q";
            exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, csv));
            Assert.Equal(CsvConverter.FIRST_LINE_ERROR, exception.Message);

            csv = "Parent,Child,Quantity\nABC,DEF,GHI";
            exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, csv));
            Assert.Equal(string.Format(CsvConverter.INVALID_QUANTITY_ERROR, 1), exception.Message);

            csv = "Parent,Child,Quantity\nABC,DEF,3\nDEF,4";
            exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, csv));
            Assert.Equal(string.Format(CsvConverter.TOKEN_COUNT_ERROR, 2), exception.Message);

            csv = "Parent,Child,Quantity\nABCD,DEFG,13";
            exception = Assert.Throws<ArgumentException>(() => GraphFactory.Generate(InputType.CSV, csv));
            Console.WriteLine(exception.Message);
            Assert.Equal("Errors encountered on line 1:\nThe field ParentName must match the regular expression '^[A-Z]{3}$'.\nThe field ChildName must match the regular expression '^[A-Z]{3}$'.\nThe field Quantity must be between 1 and 9.", exception.Message);

            csv = "Parent,Child,Quantity\nABD,DEF,3";
            var c = GraphFactory.Generate(InputType.CSV, csv);
            Assert.NotNull(c);
            
        }
    }
}
