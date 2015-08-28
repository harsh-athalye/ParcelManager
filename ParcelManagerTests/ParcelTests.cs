using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelManager;
using System;

namespace ParcelManagerTests
{
    /// <summary>
    /// This class contains Unit test cases 
    /// to check the functionality of the Parcel Rule Engine
    /// </summary>
    [TestClass]
    public class ParcelTests
    {
        /// <summary>
        /// Check if negative or zero value supplied 
        /// to Parcel object results in an error
        /// </summary>
        [TestMethod]
        public void TestInvalidValues()
        {
            try
            {
                //Arrange & Act
                Parcel p = new Parcel(-1,1,2,3);
            }
            catch(Exception ex)
            {
                //Assert
                Assert.AreEqual(ex.Message, "Invalid Value for Weight");
            }

            try
            {
                //Arrange & Act
                Parcel p = new Parcel(1, 0, 2, 3);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(ex.Message, "Invalid Value for Height");
            }

            try
            {
                //Arrange & Act
                Parcel p = new Parcel(1, 1, -2, 3);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(ex.Message, "Invalid Value for Width");
            }

            try
            {
                //Arrange & Act
                Parcel p = new Parcel(1, 1, 2, -3);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(ex.Message, "Invalid Value for Depth");
            }
        }

        /// <summary>
        /// Check if Highest rule is applied correctly when weight exceeds 50kg
        /// </summary>
        [TestMethod]
        public void TestHighestRule()
        {
            //Arrange
            Parcel p = new Parcel(51, 10, 10, 10);

            //Act
            Tuple<string, double> output = RuleEngine.ApplyRule(p);

            //Assert
            Assert.AreEqual(output.Item1, "Rejected");
            Assert.AreEqual(output.Item2, double.NaN);
        }

        /// <summary>
        /// Check if second rule is applied correctly 
        /// when weight exceeds 10kg but less than 50kg
        /// </summary>
        [TestMethod]
        public void TestSeondRule()
        {
            //Arrange
            Parcel p = new Parcel(11, 10, 10, 10);

            //Act
            Tuple<string, double> output = RuleEngine.ApplyRule(p);

            //Assert
            Assert.AreEqual(output.Item1, "Heavy Parcel");
            Assert.AreEqual(output.Item2, 15*11);
        }

        /// <summary>
        /// Check if third rule is applied correctly 
        /// when volume is less than 1500
        /// </summary>
        [TestMethod]
        public void TestThirdRule()
        {
            //Arrange
            Parcel p = new Parcel(10, 745, 2, 1);

            //Act
            Tuple<string, double> output = RuleEngine.ApplyRule(p);

            //Assert
            Assert.AreEqual(output.Item1, "Small Parcel");
            Assert.AreEqual(output.Item2, (745d*2*1) * 0.05);
        }

        /// <summary>
        /// Check if fourth rule is applied correctly 
        /// when volume is less than 2500 and >=1500
        /// </summary>
        [TestMethod]
        public void TestFourthRule()
        {
            //Arrange
            Parcel p = new Parcel(10, 750, 2, 1);

            //Act
            Tuple<string, double> output = RuleEngine.ApplyRule(p);

            //Assert
            Assert.AreEqual(output.Item1, "Medium Parcel");
            Assert.AreEqual(output.Item2, (750d * 2 * 1) * 0.04);
        }

        /// <summary>
        /// Check if lowest rule is applied correctly 
        /// when weight is <=10 kg and volume >=2500
        /// </summary>
        [TestMethod]
        public void TestLowestRule()
        {
            //Arrange
            Parcel p = new Parcel(10, 1250, 2, 1);

            //Act
            Tuple<string, double> output = RuleEngine.ApplyRule(p);

            //Assert
            Assert.AreEqual(output.Item1, "Large Parcel");
            Assert.AreEqual(output.Item2, (1250d * 2 * 1) * 0.03);
        }
    }
    
}
