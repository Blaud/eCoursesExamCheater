using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using com.jarvisniu.utils;
using Newtonsoft.Json;

namespace eCoursesExamCheater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static List<List<List<string>>> questions_answers;
        public static Boolean isThreadWorking = false;
        public static Boolean isAppClosing = false;
        [STAThread]
        static void Main()
        {
            KeyListener keyListener = new KeyListener();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {             
                string answersText = File.ReadAllText("./answers.json");
                questions_answers = JsonConvert.DeserializeObject<List<List<List<string>>>>(answersText);
                keyListener.onRelease("Ctrl+C", onPressCopy);
                keyListener.onRelease("Shift+F12", onCloseApp);
                MessageBox.Show("app started\npress Shift+F12 to stop app", "Made by blaud 'iblaudi@gmail.com'");
                Application.Run();
            } catch (Exception e) {
                    MessageBox.Show(e.ToString(), "Exception");
                    Application.Exit();
            }
        }

        static void onPressCopy()
        {
            if (!isThreadWorking)
            {
                isThreadWorking = true;
                Thread thread = new Thread(() => {
                        foreach (var answer in questions_answers.Select((value, i) => new { i, value }))
                        {
                            if (answer.value[0][0].Equals(Clipboard.GetText()))
                            {
                                Clipboard.SetText(answer.value[1][0]);
                            }
                        }
                        isThreadWorking = false;
                        Thread.CurrentThread.Abort();     
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
           
        }
        static void onCloseApp()
        {
            if (!isAppClosing)
            {
                isAppClosing = true;
                MessageBox.Show("app stopped after this window closed", "Made by blaud 'iblaudi@gmail.com'");
                Application.Exit();
            }
            
        }
    }
}
