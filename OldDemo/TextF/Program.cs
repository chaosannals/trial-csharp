using System.Text.RegularExpressions;

Regex re = new Regex(@"[A-Z]");

string a = re.Replace("UserId", (m) => " " + m.Value).Trim();
string b = re.Replace("TrustedConnection", (m) => " " + m.Value).Trim();
Console.WriteLine(a);
Console.WriteLine(b);