using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Read_Json_Content
{
    public class JsonHighlighter
    {
      
        public static void HighlightJson(RichTextBox richTextBox, string json)
        {
            // Apply the JSON syntax highlighting
            HighlightPattern(richTextBox, "\"(.*?)\"(?=:)", Color.Brown); // Key
            HighlightPattern(richTextBox, "\"(.*?)\"(?=[,}])", Color.DarkGreen); // String value
            HighlightPattern(richTextBox, @"\b\d+\b", Color.Blue); // Number
            HighlightPattern(richTextBox, @"\b(true|false)\b", Color.Purple); // Boolean
            HighlightPattern(richTextBox, @"\bnull\b", Color.Gray); // Null
            HighlightPattern(richTextBox, @"(?<=:\s*)""([^""]*)""", Color.OrangeRed); // 

         
        }

        private static void HighlightPattern(RichTextBox richTextBox, string pattern, Color color)
        {
            var matches = Regex.Matches(richTextBox.Text, pattern);
            foreach (Match match in matches)
            {
                richTextBox.Select(match.Index, match.Length);
                richTextBox.SelectionColor = color;
            }
            //richTextBox.SelectionStart = richTextBox.TextLength;
            //richTextBox.SelectionColor = richTextBox.ForeColor;
        }
    }
}
