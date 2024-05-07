using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessorCommands.Helpers
{
    /// <summary>
    /// Class for work with language in application
    /// </summary>
    public static class LanguageManager
    {
        /// <summary>
        /// Refreshes the current culture and UI culture based on the language stored in application settings.
        /// </summary>
        public static void Refresh()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(CurrentLanguage);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(CurrentLanguage);
        }

        /// <summary>
        /// Changes the language setting of the application and restarts it if necessary.
        /// </summary>
        /// <param name="lang">The language code to set.</param>
        public static void Change(string lang)
        {
            CurrentLanguage = lang;

            var oldWindows = Application.Current.Windows;
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();

            foreach (Window window in oldWindows)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Gets or sets the current language of the application.
        /// </summary>
        public static string CurrentLanguage
        {
            get 
            {
                if (Properties.Settings.Default.LanguageApp != null)
                    return Properties.Settings.Default.LanguageApp;

                return "en";
            }
            private set
            {
                if (Properties.Settings.Default.LanguageApp == value) 
                    return;

                Properties.Settings.Default.LanguageApp = value;
            }
        }

    }
}
