using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVC2MikuMikuMouth
{
    public class ErrorLogger
    {
        public static void OutputLog(string filePath, string messege)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(messege);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Windows.Forms.MessageBox.Show(e.ToString(), e.Message);
            }
        }
    }
}
