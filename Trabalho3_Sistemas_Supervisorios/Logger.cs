using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trabalho3_Sistemas_Supervisorios
{
    public static class Logger
    {
        static List<EventModel> events = new List<EventModel>();

        public static void AddSingleLog(int id, string message, DateTime time, Status status)
        {
            events.Add(new EventModel
            {
                Id = id,
                Message = message,
                Timestamp = time.ToString("hh:mm:ss"),
                Status = Enum.GetName(status.GetType(), status)
        });
        }


        public static async Task<bool?> SaveAsync(string folderPath)
        {
            var logPath = Path.Combine(folderPath, "log.txt");
            bool success = false;

            var jsonString = new string[] { JsonConvert.SerializeObject(events, Formatting.Indented) };
            try
            {
                File.WriteAllLines(logPath, jsonString);
                return success = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return success = false;
            }
            finally
            {

                await Task.Delay(500);
            }

        }

        public static bool Save(string folderPath)
        {
            var logPath = Path.Combine(folderPath, "log.txt");

            var jsonString = new string[] { JsonConvert.SerializeObject(events, Formatting.Indented) };
            try
            {
                File.WriteAllLines(logPath, jsonString);
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        class EventModel
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public string Timestamp { get; set; }
            public string Status { get; set; }

        }

        public enum Status
        {
            Normal,
            Warning,
            Error
        }
    }


}
