//------------------------------------------------------------------------------
// <copyright file="PasteCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using Microsoft.VisualStudio.Shell;
using PasterVsix.Extensions;

namespace PasterVsix
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PasteCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        public const int PasteToUnicodeCommandId = 0x0101;
        public const int PasteToUtf8CommandId = 0x0102;
        public const int PasteToAnsiCommandId = 0x0103;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("92851224-8e45-4177-baa4-730220299847");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasteCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private PasteCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this._package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(CommandSet, PasteToUnicodeCommandId);
                menuItem = new MenuCommand(this.OnPasteToUnicodeClick, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(CommandSet, PasteToUtf8CommandId);
                menuItem = new MenuCommand(this.OnPasteToUtf8Click, menuCommandID);
                commandService.AddCommand(menuItem);

                menuCommandID = new CommandID(CommandSet, PasteToAnsiCommandId);
                menuItem = new MenuCommand(this.OnPasteToAnsiClick, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PasteCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this._package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new PasteCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var text = ClipboardExtension.GetContentText();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            const string format = "string input = {0};";

            var input = string.Format(format, text.ToLiteral());

            var selection = ServiceProvider.GetCurrentTextSelection();
            if (selection == null)
            {
                return;
            }

            selection.Insert(input);
        }

        private void OnPasteToAnsiClick(object sender, EventArgs e)
        {
            InsertBytes(Encoding.ASCII);
        }


        private void OnPasteToUnicodeClick(object sender, EventArgs e)
        {
            InsertBytes(Encoding.Unicode);
        }


        private void OnPasteToUtf8Click(object sender, EventArgs e)
        {
            InsertBytes(Encoding.UTF8);
        }

        private void InsertBytes(Encoding p_encoding)
        {
            var text = ClipboardExtension.GetContentText();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            const string format = "byte[] buffer = {{ {0} }};";

            var bytes = p_encoding.GetBytes(text);

            var content = bytes.ToArrayString();
            var input = string.Format(format, content);

            var selection = ServiceProvider.GetCurrentTextSelection();
            if (selection == null)
            {
                return;
            }

            selection.Insert(input);
        }
    }
}
