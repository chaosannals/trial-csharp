using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using ExcelDemo;
using ExcelDemo.Properties;
using System.IO;

List<DemoRow> rows = new List<DemoRow>();

for(int i = 0; i < 10; i++)
{
    var bs = File.ReadAllBytes("Resources/DemoPicture.jpg");
    Console.WriteLine($"bs: {bs.Length}: {string.Join(string.Empty, bs.Take(5).Select(i => i.ToString("X")))}");
    rows.Add(new DemoRow() {
        No = i,
        Name = $"Row {i}",
        Picture=bs,
    });
}

DemoBook data = new DemoBook()
{
    Title1 = "测试标题",
    Rows = rows
};

MiniExcel.SaveAsByTemplate("Temp/result2.xlsx", File.ReadAllBytes("Resources/示例工作簿.xlsx"), data, new OpenXmlConfiguration()
{
    EnableConvertByteArray = false,
});
