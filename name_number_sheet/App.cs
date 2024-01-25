using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace RevitNameSheet
{   
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {

            RibbonPanel curPanel = a.CreateRibbonPanel("Mplugin");
            string curAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string curAssemblyPath = System.IO.Path.GetDirectoryName(curAssembly);

            PushButtonData pbd1 = new PushButtonData("Name e Number for Sheet","Name e Number" + "\r" + "for Sheet",curAssembly,"RevitNameSheet.Command");

            pbd1.LargeImage = new BitmapImage(new Uri(System.IO.Path.Combine(curAssemblyPath, "icon_1.png")));

            PushButton pb1 = (PushButton)curPanel.AddItem(pbd1);

            return Result.Succeeded; 

        }
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

    }
}