using System.Diagnostics;

namespace EndToEndTests;

public class Hooks
{
    [Before(TestSession)]
    public static void InstallPlaywright()
    {
        if (Debugger.IsAttached)
        {
            Environment.SetEnvironmentVariable("PWDEBUG", "1");
        }

        Microsoft.Playwright.Program.Main(["install"]);
    }
}