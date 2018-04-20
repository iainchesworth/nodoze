using System;
using System.Windows.Forms;

namespace NoDoze.Helpers
{
    class Support
    {
        public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string name, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);

            if (eventHandler != null)
            {
                item.Click += eventHandler;
            }

            item.Name = name;

            return item;
        }
    }
}
