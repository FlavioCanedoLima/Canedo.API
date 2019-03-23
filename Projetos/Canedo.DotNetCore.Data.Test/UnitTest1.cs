using Canedo.DotNetCore.Data.RavenDB;
using System;
using Xunit;

namespace Canedo.DotNetCore.Data.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new Start().GetRepository(); 
        }
    }
}
