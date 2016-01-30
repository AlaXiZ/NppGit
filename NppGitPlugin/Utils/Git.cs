using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NppGit.Utils
{
    public static class Git
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async static Task<string> SaveStreamToFileAsync(Stream input, string fileName, string directory = null)
        {
            if (string.IsNullOrEmpty(directory))
            {
                directory = Path.GetTempPath();
            }
            var result = Path.Combine(directory, fileName);
            try
            {
                using (var outFile = File.Create(result))
                {
                    input.Seek(0, SeekOrigin.Begin);
                    await input.CopyToAsync(outFile);
                }
            }
            catch (Exception e)
            {
                logger.Debug(e, "Stack Trace: ", e.StackTrace);
            }
            return result;
        }
    }
}
