using iText.Commons.Actions;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;

public class HeaderFooterEventHandler : AbstractPdfDocumentEventHandler
{
    private readonly string _logoPath;

    public HeaderFooterEventHandler(string logoPath)
    {
        _logoPath = logoPath;  // 📌 Stocker le chemin du logo
    }

    protected override void OnAcceptedEvent(AbstractPdfDocumentEvent docEvent)
    {
        // Convertir l'événement en PdfDocumentEvent
        PdfDocumentEvent pdfDocEvent = (PdfDocumentEvent)docEvent;
        PdfDocument pdf = pdfDocEvent.GetDocument();
        PdfPage page = pdfDocEvent.GetPage();
        PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);
        Rectangle pageSize = page.GetPageSize();

        // ✅ Ajouter le logo en haut à gauche
        if (!string.IsNullOrEmpty(_logoPath))
        {
            ImageData imageData = ImageDataFactory.Create(_logoPath);
            Image logo = new Image(imageData).ScaleToFit(150, 150);
            new Canvas(canvas, pageSize).Add(logo.SetFixedPosition(pageSize.GetLeft() + 30, pageSize.GetTop() - 60));
        }

        // Adjust header text
        canvas.BeginText()
            .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD), 14)
            .MoveText(pageSize.GetWidth() / 2 - 70, pageSize.GetTop() - 40) // Adjust centering
            .ShowText("En-tête du document")
            .EndText();

        // ✅ Ajouter un pied de page (numéro de page)
        int pageNumber = pdf.GetPageNumber(page);
        int totalPages = pdf.GetNumberOfPages();
        string footerText = $"Page {pageNumber} / {totalPages}";
        canvas.BeginText()
            .SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA), 10)
            .MoveText(pageSize.GetWidth() / 2 - 30, 20)
            .ShowText(footerText)
            .EndText();

        canvas.Release();
    }
}