using System;
using NUnit.Framework;

namespace AOPify.TDD
{
    [TestFixture]
    public class AOPifyFixture
    {
        [Test]
        public void TransactionTest()
        {
            bool called;
            AOPify.Let.WithTransaction(() =>
                                           {
                                               called = true;
                                               Assert.IsTrue(called, "Transaction succeeded");
                                           }, () =>
                                                  {
                                                      called = false;
                                                      Assert.IsFalse(called,"Transaction Failed");
                                                  }).Run(()=> Console.Write("Run Executed"));
        }
    }
}