using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace AutoCADAutomation
{
    public class Program
    {
        [CommandMethod("3DText")]
        public void create3DText()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the current space (model or layout)
                //BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                //DBDictionary groupDict = tr.GetObject(db.GroupDictionaryId, OpenMode.ForWrite) as DBDictionary;

                // Create a selection filter for Text and MText objects
                TypedValue[] filterList = new TypedValue[]
                {
                    new TypedValue(0, "TEXT,MTEXT")
                };
                SelectionFilter filter = new SelectionFilter(filterList);

                // Prompt the user to select Text and MText objects
                PromptSelectionResult selectionResult = ed.SelectAll(filter);
                if (selectionResult.Status != PromptStatus.OK)
                {
                    ed.WriteMessage("No Text or MText objects in the drawings.");
                    return;
                }

                SelectionSet selectionSet = selectionResult.Value;

                // Iterate through the selected objects
                foreach (SelectedObject selectedObject in selectionSet)
                {
                    if (selectedObject.ObjectId.ObjectClass.Name == "AcDbText" ||
                        selectedObject.ObjectId.ObjectClass.Name == "AcDbMText")
                    {
                        Entity obj = tr.GetObject(selectedObject.ObjectId, OpenMode.ForWrite) as Entity;
                        // get text or mtext insert point
                        // get text from text or mtext

                        Point3d xpos = new Point3d(0, 0, 0);
                        string xstring = "teststring";
                        double xrot = 0;
                        Vector3d xnormal = new Vector3d(0, 0, 0);

                        if (obj is DBText textEntity)
                        {
                            // Read text value, position, and rotation
                            xstring = textEntity.TextString;
                            xpos = textEntity.Position;
                            //xrot = textEntity.Rotation;
                            xnormal = textEntity.Normal;

                        }
                        else if (obj is MText mtextEntity)
                        {
                            // Read text value, position, and rotation
                            xstring = mtextEntity.Contents;
                            xpos = mtextEntity.Location;
                            //xrot = mtextEntity.Rotation;
                            xnormal = mtextEntity.Normal;
                        }

                        //ed.WriteMessage("\nxstring: " + xstring);
                        //ed.WriteMessage("\nxpos.X: " + xpos.X);

                        
                        //Group group = new Group();

                        for (int x = 0; x < xstring.Length; x++)
                        {
                            string xchar = xstring[x].ToString();

                            xpos = new Point3d(xpos.X + 2.5, xpos.Y, xpos.Z);

                            double[] inarray;

                            try
                            {
                                inarray = xFont(xchar.ToUpper());
                            }
                            catch (System.Exception e)
                            {
                                inarray = xFont("_");
                            }


                            for (int y = 0; y < inarray.Length; y += 4)
                            {
                                Line line = new Line(new Point3d(xpos.X + inarray[y], xpos.Y + inarray[y + 1], xpos.Z), new Point3d(xpos.X + inarray[y + 2], xpos.Y + inarray[y + 3], xpos.Z));
                                btr.AppendEntity(line);
                                tr.AddNewlyCreatedDBObject(line, true);
                                //group.Append(line.ObjectId);
                            }
                        }


                        //groupDict.SetAt(xstring, group);


                    }
                }

                tr.Commit();
            }
        }

        public static double[] xFont(string charin)
        {
            Dictionary<string, double[]> Font = new Dictionary<string, double[]>();
            Font["0"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 0, 0, 1, 2, 0, 0 };
            Font["1"] = new double[] { 1, 2, 1, 0 };
            Font["2"] = new double[] { 0, 1, 1, 1, 0, 2, 1, 2, 1, 0, 0, 0, 1, 2, 1, 1, 0, 0, 0, 1 };
            Font["3"] = new double[] { 0, 1, 1, 1, 0, 2, 1, 2, 1, 0, 0, 0, 1, 2, 1, 0 };
            Font["4"] = new double[] { 0, 1, 1, 1, 0, 1, 0, 2, 1, 2, 1, 0 };
            Font["5"] = new double[] { 0, 1, 1, 1, 0, 1, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0, 1, 1, 1, 0 };
            Font["6"] = new double[] { 0, 1, 1, 1, 0, 0, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0, 1, 1, 1, 0 };
            Font["7"] = new double[] { 0, 2, 1, 2, 1, 2, 1, 0 };
            Font["8"] = new double[] { 0, 1, 1, 1, 0, 0, 0, 2, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 0, 0 };
            Font["9"] = new double[] { 0, 1, 1, 1, 0, 1, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0, 1, 2, 1, 0 };
            Font["A"] = new double[] { 0.5, 2, 0, 0, 0.25, 1, 0.75, 1, 0.5, 2, 1, 0 };
            Font["B"] = new double[] { 0, 1, 1, 1, 0, 0, 0, 2, 0, 2, 0.75, 2, 1, 0, 0, 0, 0.75, 2, 0.75, 1, 1, 0, 1, 1 };
            Font["C"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0 };
            Font["D"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 1.75, 1, 0.25, 0, 0, 1, 1.75, 1, 0.25 };
            Font["E"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0, 0, 1, 1, 1 };
            Font["F"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 0, 1, 1, 1 };
            Font["G"] = new double[] { 0, 0, 0, 1.75, 1, 0, 0, 0, 0, 1.75, 1, 2, 0.5, 1, 1, 1, 1, 1, 1, 0 };
            Font["H"] = new double[] { 0, 0, 0, 2, 0, 1, 1, 1, 1, 2, 1, 0 };
            Font["I"] = new double[] { 0, 2, 1, 2, 1, 0, 0, 0, 0.5, 2, 0.5, 0 };
            Font["J"] = new double[] { 0, 2, 1, 2, 1, 0.25, 0, 0, 1, 2, 1, 0.25 };
            Font["K"] = new double[] { 0, 0, 0, 2, 0, 1, 1, 2, 1, 0, 0, 1 };
            Font["L"] = new double[] { 0, 0, 0, 2, 1, 0, 0, 0 };
            Font["M"] = new double[] { 0, 0, 0, 2, 0, 2, 0.5, 1, 1, 2, 0.5, 1, 1, 2, 1, 0 };
            Font["N"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 0, 1, 2, 1, 0 };
            Font["O"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 0, 0 };
            Font["P"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 0, 1, 1, 1, 1, 2, 1, 1 };
            Font["Q"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0 };
            Font["R"] = new double[] { 0, 0, 0, 2, 0, 2, 1, 2, 0, 1, 1, 1, 1, 2, 1, 1, 0, 1, 1, 0 };
            Font["S"] = new double[] { 0, 1, 0, 2, 0, 2, 1, 2, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0 };
            Font["T"] = new double[] { 0, 2, 1, 2, 0.5, 2, 0.5, 0 };
            Font["U"] = new double[] { 0, 0, 0, 2, 1, 2, 1, 0, 1, 0, 0, 0 };
            Font["V"] = new double[] { 0.5, 0, 0, 2, 1, 2, 0.5, 0 };
            Font["W"] = new double[] { 0, 2, 0.25, 0, 0.25, 0, 0.5, 1, 0.75, 0, 0.5, 1, 1, 2, 0.75, 0 };
            Font["X"] = new double[] { 1, 0, 0, 2, 0, 0, 1, 2 };
            Font["Y"] = new double[] { 0.5, 1, 0, 2, 1, 2, 0, 0 };
            Font["Z"] = new double[] { 0, 2, 1, 2, 1, 0, 0, 0, 0, 0, 1, 2 };
            Font["_"] = new double[] { 1, 0, 0, 0 };
            Font["."] = new double[] { 0.625, 0, 0.375, 0, 0.625, 0.25, 0.375, 0.25, 0.375, 0.25, 0.375, 0, 0.625, 0.25, 0.625, 0 };
            Font["-"] = new double[] { 0, 1, 1, 1 };
            return Font[charin];
        }



    }
}
