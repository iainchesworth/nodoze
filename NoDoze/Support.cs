using System;
using System.Windows.Forms;

namespace NoDoze
{
    class Support
    {
        static public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string name, EventHandler eventHandler)
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
