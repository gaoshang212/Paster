
using EnvDTE;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider = System.IServiceProvider;

namespace PasterVsix.Extensions
{
    public static class ServiceProviderExtension
    {
        public static TextSelection GetCurrentTextSelection(this IServiceProvider p_serviceProvider)
        {
            var dte = p_serviceProvider.GetService(typeof(SDTE)) as DTE;


            if (dte == null
                || dte.SelectedItems.Count <= 0)
            {
                return null;
            }

            var td = dte.ActiveDocument;

            var ts = td.Selection as TextSelection;

            return ts;
        }
    }
}