/************************************************************************
 * * Copyright © 2015 南宁市国土资源信息中心 All rights reserved.
 * *
 * CLR:         4.0.30319.18408
 * Machine:     XINXI-FENGZJ
 * File:        PointSelectEventHandler
 * GUID:        93682cc4-343e-4a76-ad30-03472aa702d7
 * Domain:      NLIS
 * User:        fengzhenjian 
 * ----------------------------------------------------------------------
 * Depiction:   
 * Author:      Von_dream
 * CDT:         2015/5/1 15:03:37
 * Version:     1.0.0.1
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using Bentley.Interop.MicroStationDGN;
using Bentley.MicroStation.InteropServices;
using Application = Bentley.Interop.MicroStationDGN.Application;
using View = Bentley.Interop.MicroStationDGN.View;

namespace Nlic.MicroStation.Interop{

    public class PointSelectEventHandler : IPrimitiveCommandEvents{
        private static readonly Application App = Utilities.ComApp;

        public void Cleanup(){
        }

        public void DataPoint(ref Point3d Point, View View){
            var oElm = App.CommandState.LocateElement(Point, View, true);
            if (oElm != null){
                if (oElm.IsTextElement()){
                    var txtElem = oElm.AsTextElement();
                    txtElem.TextStyle.Justification = MsdTextJustification.CenterCenter;
                }
            }
        }

        public void Dynamics(ref Point3d point, View view, MsdDrawingMode drawMode){
        }

        public void Keyin(string Keyin){
        }

        public void Reset(){
        }

        public void Start(){
//            App.CommandState.SetLocateCursor();
        }
    }
}
