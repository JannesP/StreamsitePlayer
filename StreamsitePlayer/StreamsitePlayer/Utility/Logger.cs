using StreamsitePlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer
{
    class Logger
    {
        private static bool errorOccured = false;
        private static LoggerInstance instance = null;
        private static string logFile;
        private const string LOG_PATH = @"logs\";
        static Logger()
        {
            string logFile = LOG_PATH;
            logFile += DateTime.Now.Year.ToString("0000");   //yyyy
            logFile += DateTime.Now.Day.ToString("00");    //yyyydd
            logFile += (DateTime.Now.Month + 1).ToString("00");    //yyyyddmm
            logFile += "-" + DateTime.Now.Hour.ToString("00"); //yyyyddmm-hh
            logFile += DateTime.Now.Minute.ToString("00"); //yyyyddmm-hhmm
            logFile += DateTime.Now.Second.ToString("00"); //yyyyddmm-hhmmss
            logFile += DateTime.Now.Millisecond.ToString("000"); //yyyyddmm-hhmmssmmm
            logFile += ".log";  //yyyyddmm-hhmmssmmm.log
            Logger.logFile = Path.Combine(Environment.CurrentDirectory, logFile);
            try
            {
                if (!Directory.Exists(LOG_PATH))
                {
                    Directory.CreateDirectory(LOG_PATH);
                }
                File.Create(Logger.logFile).Close();
                if (!Util.CheckWriteAccess(Logger.logFile)) throw new Exception();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Log file couldn't be created.\nCheck the permissions or start the program as admin.\nWe need write access to the executable directory!\n" + ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            instance = new LoggerInstance(logFile);
        }

        public static void Log(string prefix, string message)
        {
            if (errorOccured) return;
            if (instance != null)
            {
                instance.Log(prefix, message);
            }
            else   //LoggerInstance is null. Should never happen.
            {
                errorOccured = true;
                MessageBox.Show("The LoggerInstance instance is null!\n Please report this error.\nThe program will nevertheless work like always.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        private class LoggerInstance
        {
            private string fileName;
            private StreamWriter writer;

            public LoggerInstance(string fileName)
            {
                this.fileName = fileName;
                this.writer = CreateWriter(fileName);
            }

            /// <summary>
            /// Creates a new instance of a StreamWriter with the given filePath.
            /// </summary>
            /// <param name="fileName">The file to log into</param>
            /// <returns>The new instance</returns>
            private static StreamWriter CreateWriter(string fileName)
            {
                StreamWriter writer = new StreamWriter(fileName, true);
                writer.NewLine = "\n";
                writer.AutoFlush = true;
                return writer;
            }

            public void Log(string prefix, string message)
            {
                string logLine = CreateTimeString() + "\t" + prefix + "\t" + message + "\n";
#if DEBUG
                Console.Write(logLine);
#endif
                //write to file
                Thread t = new Thread(() => WriteToFile(logLine));
                t.Start();
            }

            private void WriteToFile(string line)
            {
                if (writer == null)
                {
                    writer = CreateWriter(fileName);
                }
                writer.Write(line);
            }
        }

        private static string CreateTimeString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
    }

    
}
