/*--------------------------------------------------------------------------------------+
|
|     $Source: KeyinCommands.cs,v $
|    $RCSfile: Keyincommands].cs,v $
|   $Revision: 1.1 $
|
|  $Copyright: (c) 2006 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

using System;
using System.Windows.Forms;
using BCOM = Bentley.Interop.MicroStationDGN;
using BMI = Bentley.MicroStation.InteropServices;

namespace MyMicroStationTools {
    /// <summary>Class used for running key-ins.  The key-ins
    /// XML file commands.xml provides the class name and the method names.
    /// </summary>
    internal class KeyinCommands {

        public static void MyMicroStationToolsCommand(String unparsed) {
            var mainForm = FormMain.GetInstance();

            //            mainForm.Text = GetSystemName();
            //                                        mainForm.userLabelBarItem.Text = ParamDictionary.ActiveUserName;
            mainForm.AttachAsTopLevelForm(MyMicroStationTools.MyAddin, false);
            mainForm.NETDockable = true;
            mainForm.Show();
        }

        public static void MyMicroStationToolsPlacementCommand(String unparsed) {

        }

    }  // End of KeyinCommands

}  // End of the namespace
