using NUnit.Framework;
using System.Text;
using System.Text.Unicode;
using TaskDNS.Application.Processes;

namespace TaskDNS.Tests
{

    public class WorkDirectoryTests
    {
        static object[] Parametrs_WorkDirectory_GetDirectory_TreeString_Ok =
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

        [TestCaseSource(nameof(Parametrs_WorkDirectory_GetDirectory_TreeString_Ok))]
        public void WorkDirectory_GetDirectory_TreeString_Ok(string path,string command,string expectedOutput)
        {
            var result = ProcessPathHandler.GetDirectory(path, command);

            Assert.AreEqual(expectedOutput, result);
        }
    }
}