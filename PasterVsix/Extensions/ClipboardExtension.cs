using System.IO;
using System.Windows.Forms;

namespace PasterVsix.Extensions
{
    public class ClipboardExtension
    {
        /// <summary>
        /// 获取剪切版内容
        /// </summary>
        /// <returns></returns>
        public static string GetContentText(ClipboardFileMode mode = ClipboardFileMode.Path)
        {
            string text;
            do
            {
                var files = Clipboard.GetFileDropList();
                if (files.Count > 0)
                {
                    text = files[0];

                    if (mode == ClipboardFileMode.Content
                        && File.Exists(text))
                    {
                        text = File.ReadAllText(text);
                    }

                    break;
                }

                text = Clipboard.GetText();

            } while (false);

            return text;
        }
    }

    public enum ClipboardFileMode
    {
        Path,
        Content,
    }
}