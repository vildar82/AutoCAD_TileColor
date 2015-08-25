using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class Panel
   {
      // Panel - объект панели для каждого блока(ref) панели в чертеже (с уникальным objectId).
            
      ObjectId _idBlRef;

      public Panel(BlockReference blRef)
      {
         _idBlRef = blRef.ObjectId;         
      }      
   }
}
