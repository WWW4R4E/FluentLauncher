using FluentLauncher.Core.Commands;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.Storage.Pickers;
using FluentLauncher.Core.Models;

namespace FluentLauncher.Controls
{
    public sealed partial class JDKPathControl : UserControl
    {
        public List<JavaPath> JdkPaths;

        public JDKPathControl()
        {
            JdkPaths = new List<JavaPath>();
            this.InitializeComponent();
        }

        private async void Button_SelectJDK(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add(".exe");
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ProcessSelectedJDK(file.Path);
            }
        }

        private void Button_SearchJDK(object sender, RoutedEventArgs e)
        {
            var potentialPaths = GetPotentialJDKPathsFromEnvironment();
            foreach (var path in potentialPaths)
            {
                ProcessSelectedJDK(path);
            }
        }

        private void ProcessSelectedJDK(string javaExePath)
        {
            if (!JdkPaths.Any(j=>j.Path == javaExePath))
            {
                var jdkVersion = GetJdkVersion(javaExePath);
                if (jdkVersion != "未知版本")
                {
                    JdkPaths.Add(new JavaPath()  { Path = javaExePath ,  Version = jdkVersion});
                    if (JDK_ComBox is ComboBox combobox)
                    {
                        combobox.Items.Add(jdkVersion);
                    }
                }
            }
        }

        private List<string> GetPotentialJDKPathsFromEnvironment()
        {
            var paths = new List<string>();

            string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(javaHome) && Directory.Exists(javaHome))
            {
                paths.Add(javaHome);
            }

            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(pathEnv))
            {
                foreach (var path in pathEnv.Split(';'))
                {
                    string trimmedPath = path.Trim();
                    if (trimmedPath.Contains("java", StringComparison.OrdinalIgnoreCase) ||
                        trimmedPath.Contains("jdk", StringComparison.OrdinalIgnoreCase))
                    {
                        string potentialRoot = FindJdkRoot(trimmedPath);
                        if (!string.IsNullOrEmpty(potentialRoot) && !paths.Contains(potentialRoot))
                        {
                            paths.Add(potentialRoot);
                        }
                    }
                }
            }

            return paths;
        }

        private string FindJdkRoot(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    foreach (var dir in Directory.GetDirectories(path))
                    {
                        if (dir.Contains("bin", StringComparison.OrdinalIgnoreCase))
                        {
                            string javaExePath = Path.Combine(dir, "java.exe");
                            if (File.Exists(javaExePath))
                            {
                                return javaExePath;
                            }
                        }
                    }
                }

                string parentPath = Path.GetFullPath(Path.Combine(path, ".."));
                if (parentPath != path)
                {
                    return FindJdkRoot(parentPath);
                }
            }
            catch
            {

            }

            return null;
        }

        private string GetJdkVersion(string javaExePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = javaExePath,
                Arguments = "-version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        var versionMatch = Regex.Match(output, @"\""?(\d+)(?:\.\d+)?(?:\.\d+)?(?:\.\d+)?");
                        if (versionMatch.Success && versionMatch.Groups.Count > 1)
                        {
                            return versionMatch.Groups[1].Value;
                        }
                    }
                }
            }
            catch
            {
            }

            return "未知版本";
        }
    }
}