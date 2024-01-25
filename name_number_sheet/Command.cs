using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using Autodesk.Revit.ApplicationServices;

namespace RevitNameSheet
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                // Obter o documento ativo do Revit
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Application app = uiapp.Application;
                Document doc = uidoc.Document;
                
                

                // Obter a ViewSheet atual
                ViewSheet currentSheet = uidoc.ActiveView as ViewSheet;

                if (currentSheet != null)
                    // Iniciar uma transação
                    using (Transaction trans = new Transaction(doc, "Adicionar QR Code"))
                    {
                        trans.Start();

                    // Número da folha
                    string sheetNumber = currentSheet.SheetNumber;

                    // Nome da folha
                    string sheetName = currentSheet.Name;

                    // Adicionar número da folha como TextNote
                    XYZ numberInsertionPoint = new XYZ(1.10, 0.11, 0); // Ajuste conforme necessário
                    TextNote.Create(doc, currentSheet.Id, numberInsertionPoint, "A" + sheetNumber, GetDefaultTextTypeId(doc));

                    // Adicionar nome da folha como TextNote
                    XYZ nameInsertionPoint = new XYZ(0.56, 0.05, 0); // Ajuste conforme necessário
                    TextNote.Create(doc, currentSheet.Id, nameInsertionPoint, sheetName, GetDefaultTextTypeId(doc));

                        trans.Commit();
                        trans.Dispose();
                }
                else
                {
                    TaskDialog.Show("Erro", "Esta não é uma ViewSheet válida.");
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        // Função para obter o ID do estilo de texto padrão
        private ElementId GetDefaultTextTypeId(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            // Substitua 2791991 pelo ID da fonte desejada
            ElementId desiredFontId = new ElementId(2791991);

            // Tente encontrar a fonte pelo ID
            var textNoteType = collector.Cast<TextNoteType>().FirstOrDefault(t => t.Id == desiredFontId);

            // Se a fonte for encontrada, retorne o ID
            if (textNoteType != null)
                return textNoteType.Id;

            // Se a fonte não for encontrada, retorne o primeiro TextNoteType encontrado
            return collector.FirstElementId();
        }
    }
}
