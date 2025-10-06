namespace Pepro.Presentation;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        while (true)
        {
            int employeeId;
            using (LoginForm loginForm = new())
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    break;
                }
                employeeId = loginForm.EmployeeId;
            }

            using MainForm menuForm = new() { EmployeeId = employeeId };
            if (menuForm.ShowDialog() != DialogResult.Retry)
            {
                break;
            }
        }
    }
}
