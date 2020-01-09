using Facemash.API.Business;
using System;
using Xunit;

namespace XUnitTestProject
{
    public class CatManagementTest
    {
        [Fact]
        public void Test1()
        {
            var catManagement = IOC.Get<ICatManagement>();
        }
    }
}
