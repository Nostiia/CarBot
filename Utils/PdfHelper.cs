using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarBot.Utils
{
    public static class PdfHelper
    {
        public static byte[] GeneratePdfFromText(string text)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Content()
                        .Padding(10)
                        .Text(text);
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
