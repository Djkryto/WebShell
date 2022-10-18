using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TaskDNS.App;
using TaskDNS.Channels;
using TaskDNS.Controllers;
using TaskDNS.Tools;

namespace TaskDNS.Tests
{
   
    public class Tests
    {
        static object[] ProcessingParapetrsPathCommandCD =
        {
            new object[] {@"C:\","CD Program Files",@"C:\Program Files"},
            new object[] {@"C:\",@"CD Program Files\Git", @"C:\Program Files\Git" },
            new object[] {@"C:\",@"CD ./Program Files\Git", @"C:\Program Files\Git" },
            new object[] {@"C:\",@"CD ./Program Files\ Git", @"C:\Program Files\Git" },
            new object[] {@"C:\",@"CD ./Program Files \ Git", @"C:\Program Files\Git" },
            new object[] {@"C:\",@" CD ./Program Files \ Git", @"C:\Program Files\Git" },

            new object[] { "C:\\Program Files", @"CD Git", "C:\\Program Files\\Git" },
            new object[] { "C:\\Program Files", @"CD  Git", "C:\\Program Files\\Git" },
            new object[] { "C:\\Program Files", @"CD  Git ", "C:\\Program Files\\Git" },
            new object[] { "C:\\Program Files", @" CD Git ", "C:\\Program Files\\Git" },

            new object[] { "C:\\", @"CD C:\Program Files\Git", "C:\\Program Files\\Git" },
            new object[] { "C:\\", @"CD ./C:\Program Files\Git", "C:\\Program Files\\Git" },
            new object[] { "C:\\Program Files", @" CD C:\Program Files\Git ", "C:\\Program Files\\Git" },

            new object[] { "C:\\Program Files", @" CD ../../ ", "C:\\" },
            new object[] { "C:\\Program Files\\Git", @" CD ../../ ", "C:\\" },
            new object[] { "C:\\", @" CD ../../ ", "C:\\" },

            new object[] { "C:\\", @" CD C:\..GoyPavel\Новая папка\Новая папка\Новая папка", "C:\\..GoyPavel\\Новая папка\\Новая папка\\Новая папка" },
            new object[] { "C:\\", @" CD C:\..GoyPavel\Новая папка\Новая папка\Новая папка ", "C:\\..GoyPavel\\Новая папка\\Новая папка\\Новая папка" },
            new object[] { "C:\\", @" CD  C:\..GoyPavel\Новая папка\Новая папка\Новая папка", "C:\\..GoyPavel\\Новая папка\\Новая папка\\Новая папка" },
             new object[] { "C:\\", @" CD ..GoyPavel\Новая папка\Новая папка\Новая папка", "C:\\..GoyPavel\\Новая папка\\Новая папка\\Новая папка" },

            new object[] {@"C:\","CD ..",@"C:\"},
            new object[] {@"C:\Program Files","CD ..",@"C:\"},
            new object[] {@"C:\Program Files\Git","CD ..",@"C:\Program Files"},
            new object[] {@"C:\Program Files\Git\bin","CD ..",@"C:\Program Files\Git"},

            new object[] {@"C:\","CD /",@"C:\"},
            new object[] {@"C:\Program Files","CD /",@"C:\"},
            new object[] {@"C:\Program Files\Git","CD /",@"C:\"},

            new object[] { "C:\\Program Files", @" CD ../../ ", "C:\\" },
            new object[] { "C:\\Program Files\\Git", @" CD ../../ ", "C:\\" },
            new object[] { "C:\\", @" CD ../../ ", "C:\\" },

            new object[] { "C:\\Program Files", @"Tree", "C:\\Program Files" },
            new object[] { "C:\\Program Files\\Git", @" TREE  ", "C:\\Program Files\\Git" },
            new object[] { "C:\\Program Files\\Git", @" help ", "C:\\Program Files\\Git" },
            new object[] { "C:\\", @"CD", "C:\\" },
            new object[] { "C:\\", @" CD ", "C:\\" },
        };

        private ProcessingCommand process;

        [SetUp]
        public void Setup()
        {
            process = new ProcessingCommand();
        }

        [TestCaseSource(nameof(ProcessingParapetrsPathCommandCD))]
        public void GetPropertiProcessingPathCommandCD(string path,string command,string expectedOutput)
        {
            string result = process.Processing(path, command);

            Assert.AreEqual(expectedOutput, result);
        }

      
    }
}