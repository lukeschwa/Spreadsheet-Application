// Luke Schauble.
// 11510454.

// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
namespace NUnit.Tests1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Spreadsheet_Luke_Schauble;
    using SpreadsheetEngine;

    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// Name:SetTextTest
        /// Description: Tests whether or not a cell is setting text to the correct value.
        /// </summary>
        [Test]
        public void SetTextTest()
        {
            Spreadsheet testSpread = new Spreadsheet(50, 26);

            testSpread.GetCell(0, 0).Text = "(0,0)";
            testSpread.GetCell(5, 5).Text = "(5,5)";
            testSpread.GetCell(10, 10).Text = "(10,10)";
            testSpread.GetCell(20, 5).Text = "(20,5)";

            Assert.That("(0,0)", Is.EqualTo(testSpread.GetCell(0, 0).Text), "Incorrect Text In Cell");
            Assert.That("(5,5)", Is.EqualTo(testSpread.GetCell(5, 5).Text), "Incorrect Text In Cell");
            Assert.That("(0,0)", Is.EqualTo(testSpread.GetCell(10, 10).Text), "Incorrect Text In Cell");
            Assert.That("(0,0)", Is.EqualTo(testSpread.GetCell(20, 5).Text), "Incorrect Text In Cell");
        }

        /// <summary>
        /// Name: SetValueTest
        /// Description: Tests whether or not a cell is setting value to the correct value.
        /// </summary>
        [Test]
        public void SetValueTest()
        {
            Spreadsheet testSpread = new Spreadsheet(50, 26);

            testSpread.SetValueTestHelper();

            Assert.That("Test1", Is.EqualTo(testSpread.GetCell(0, 0).Value), "Incorrect Value In Cell");
            Assert.That("Test2", Is.EqualTo(testSpread.GetCell(5, 5).Value), "Incorrect Value In Cell");
            Assert.That("Test3", Is.EqualTo(testSpread.GetCell(10, 10).Value), "Incorrect Value In Cell");
            Assert.That("Test4", Is.EqualTo(testSpread.GetCell(20, 5).Value), "Incorrect Value In Cell");
        }

        /// <summary>
        /// Name: RowCountTest.
        /// Description: Tests the RowCount method to see if it is returning the correct number of rows.
        /// </summary>
        [Test]

        public void RowCountTest()
        {
            Spreadsheet testSpread1 = new Spreadsheet(50, 26);
            Spreadsheet testSpread2 = new Spreadsheet(10, 26);
            Spreadsheet testSpread3 = new Spreadsheet(0, 26);
            Spreadsheet testSpread4 = new Spreadsheet(-1, 26);

            Assert.That("50", Is.EqualTo(testSpread1.RowCount.ToString()), "Incorrect amount of rows");
            Assert.That("10", Is.EqualTo(testSpread2.RowCount.ToString()), "Incorrect amount of rows");
            Assert.That("0", Is.EqualTo(testSpread3.RowCount.ToString()), "Incorrect amount of rows");
            Assert.That("-1", Is.EqualTo(testSpread4.RowCount.ToString()), "Incorrect amount of rows");
        }

        /// <summary>
        /// Name: ColumnCountTest
        /// Description: Tests the ColumnCount method to see if it is returning the correct number of columns.
        /// </summary>
        [Test]

        public void ColumnCountTest()
        {
            Spreadsheet testSpread1 = new Spreadsheet(50, 26);
            Spreadsheet testSpread2 = new Spreadsheet(50, 10);
            Spreadsheet testSpread3 = new Spreadsheet(50, 0);
            Spreadsheet testSpread4 = new Spreadsheet(50, -1);

            Assert.That("50", Is.EqualTo(testSpread1.ColumnCount.ToString()), "Incorrect amount of Columns");
            Assert.That("10", Is.EqualTo(testSpread2.ColumnCount.ToString()), "Incorrect amount of Columns");
            Assert.That("0", Is.EqualTo(testSpread3.ColumnCount.ToString()), "Incorrect amount of Columns");
            Assert.That("-1", Is.EqualTo(testSpread4.ColumnCount.ToString()), "Incorrect amount of Column");
        }

        /// <summary>
        /// Name: BackroundColorTest.
        /// Description: Tests changing of backround color functionality.
        /// </summary>
        [Test]

        public void BackgroundColorTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            testSpread.GetCell(0, 0).BGColor = (uint)ConsoleColor.Green;
            Assert.AreEqual(testSpread.GetCell(0, 0).BGColor, (uint)ConsoleColor.Green);
        }

        /// <summary>
        /// Name: UndoTest.
        /// Description: Tests Undo functionality.
        /// </summary>
        [Test]

        public void UndoTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            Cell testcell = testSpread.GetCell(0, 0);
            testSpread.GetCell(0, 0).Text = "1";
            testSpread.GetCell(0, 0).Text = "2";
            testSpread.Undo();
            Assert.AreEqual("1", testSpread.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Name: RedoTest.
        /// Description: Tests Redo functionality.
        /// </summary>
        [Test]
        public void RedoTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            Cell testcell = testSpread.GetCell(0, 0);
            testSpread.GetCell(0, 0).Text = "1";
            testSpread.GetCell(0, 0).Text = "2";
            testSpread.Undo();
            testSpread.Redo();
            Assert.AreEqual("2", testSpread.GetCell(0, 0).Text);
        }

        /// <summary>
        /// Name: LoadTest.
        /// Description: tests the load function.
        /// </summary>
        [Test]
        public void LoadTest()
        {
            Spreadsheet tempSpread = new Spreadsheet(50, 26);

            FileStream file = File.Open(Directory.GetCurrentDirectory() + "testXML.xml", FileMode.Open);
            tempSpread.Load(file);
            file.Close();

            Assert.AreEqual(tempSpread.GetCell(0, 0).Text, "Test");
        }

        [Test]
        /// <summary>
        /// Name: SaveTest.
        /// Description: tests the save function.
        /// MUST SAVE FILE AND NAME IT SaveTest.xml BEFORE RUNNING TEST. Can't figure out how to do it any other way.
        /// </summary>
        public void SaveTest()
        {
            bool check = false;
            if (File.Exists("SaveTest.xml"))
            {
                check = true;
            }

            Assert.That(true, Is.EqualTo(check));
        }

        /// <summary>
        /// Name: SelfReferenceTest.
        /// Description: Tests detection of a self reference.
        /// </summary>
        [Test]
        public void SelfReferenceTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            testSpread.GetCell(0, 0).Text = "A1";
            Assert.That(testSpread.GetCell(0, 0).Value, Is.EqualTo("!(Self Reference)"));
        }

        /// <summary>
        /// Name: BadReferenceTest.
        /// Description: Tests detection of a bad reference.
        /// </summary>
        [Test]
        public void BadReferenceTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            testSpread.GetCell(0, 0).Text = "fjdsfkjdkfksjd";
            Assert.That(testSpread.GetCell(0, 0).Value, Is.EqualTo("!(Bad Reference)"));
        }

        /// <summary>
        /// Name: CircularReferenceTest.
        /// Description: Tests detection of a circular reference.
        /// </summary>
        [Test]
        public void CircularReferenceTest()
        {
            Spreadsheet testSpread = new Spreadsheet(5, 5);
            testSpread.GetCell(0, 0).Text = "B1";
            testSpread.GetCell(0, 1).Text = "A1";
            Assert.That(testSpread.GetCell(0, 1).Value, Is.EqualTo("!(Circular Reference)"));
        }
    }
}
