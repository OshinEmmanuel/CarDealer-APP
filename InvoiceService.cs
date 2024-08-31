using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

public class InvoiceService
{
    public void GenerateInvoice(User user, Car car, string firstName, string lastName, string email, string phone, string address, string cardNumber)
    {
        // Generate Text Invoice
        var invoiceFileName = $"Invoice_{user.Username}_{DateTime.Now.Ticks}.txt";
        using (StreamWriter writer = new StreamWriter(invoiceFileName))
        {
            WriteInvoiceContent(writer, user, car, firstName, lastName, email, phone, address, cardNumber);
        }

        // Generate PDF Invoice
        var pdfInvoiceFileName = $"Invoice_{user.Username}_{DateTime.Now.Ticks}.pdf";
        using (var writer = new PdfWriter(pdfInvoiceFileName))
        {
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            WritePdfInvoiceContent(document, user, car, firstName, lastName, email, phone, address, cardNumber);
            document.Close();
        }

        Console.WriteLine($"Text invoice generated: {invoiceFileName}");
        Console.WriteLine($"PDF invoice generated: {pdfInvoiceFileName}");
    }

    private void WriteInvoiceContent(TextWriter writer, User user, Car car, string firstName, string lastName, string email, string phone, string address, string cardNumber)
    {
        // Invoice Header
        writer.WriteLine("===========================================");
        writer.WriteLine("                 INVOICE                   ");
        writer.WriteLine("===========================================");
        writer.WriteLine();
        writer.WriteLine($"Date: {DateTime.Now:yyyy-MM-dd}");
        writer.WriteLine($"Invoice #: {DateTime.Now.Ticks}");
        writer.WriteLine();

        // Seller Details
        writer.WriteLine("Seller:");
        writer.WriteLine("Kenny Car Dealership");
        writer.WriteLine("1234 Elm Street, London, UK");
        writer.WriteLine("Chelsea, State, ZIP");
        writer.WriteLine("Phone: (123) 456-7890");
        writer.WriteLine();

        // Buyer Details
        writer.WriteLine("Buyer:");
        writer.WriteLine($"Name: {user.Username}");
        writer.WriteLine($"FullName: {firstName} {lastName}");
        writer.WriteLine($"Address: {address}");
        writer.WriteLine($"Contact: {phone}");
        writer.WriteLine($"Email: {email}");
        writer.WriteLine();

        // Car Details
        writer.WriteLine("Car Details:");
        writer.WriteLine("-------------------------------------------");
        writer.WriteLine($"Lot Number: {car.LotNumber}");
        writer.WriteLine($"Make: {car.Make}");
        writer.WriteLine($"Model: {car.Model}");
        writer.WriteLine($"Year: {car.Year}");
        writer.WriteLine($"Price: ${car.Price}");
        writer.WriteLine("-------------------------------------------");
        writer.WriteLine();

        // Payment Information
        writer.WriteLine("Payment Information:");
        writer.WriteLine("Payment Method: Online");
        writer.WriteLine($"Card Number: {cardNumber}");
        writer.WriteLine("Total Amount Due: $" + car.Price.ToString("F2"));
        writer.WriteLine();

        // Footer and Signature Line
        writer.WriteLine("===========================================");
        writer.WriteLine("Thank you for your business!");
        writer.WriteLine();
        writer.WriteLine($"Signature: __________________");
        writer.WriteLine();
        writer.WriteLine("Please make all checks payable to Kenny Car Dealership.");
        writer.WriteLine("If you have any questions concerning this invoice, contact our office at (123) 456-7890.");
        writer.WriteLine();
        writer.WriteLine("===========================================");
    }

    private void WritePdfInvoiceContent(Document document, User user, Car car, string firstName, string lastName, string email, string phone, string address, string cardNumber)
    {
        // Set up the document with a custom page size (e.g., A5)
        var pageSize = PageSize.A5;
        document.SetMargins(20, 20, 20, 20);
        
        // Define colors and fonts
        Color headerColor = ColorConstants.BLUE;
        Color bodyColor = ColorConstants.BLACK;

        PdfFont headerFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
        PdfFont bodyFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

        // Invoice Header
        document.Add(new Paragraph("INVOICE")
            .SetFont(headerFont)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(headerColor)
            .SetMarginBottom(10));
        
        document.Add(new Paragraph($"Date: {DateTime.Now:yyyy-MM-dd}")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontColor(bodyColor));
        document.Add(new Paragraph($"Invoice #: {DateTime.Now.Ticks}")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontColor(bodyColor));
        
        document.Add(new Paragraph("\n"));

        // Seller Details
        Table sellerTable = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
        sellerTable.AddCell(new Cell().Add(new Paragraph("Seller: Kenny Car Dealership")
            .SetFont(headerFont)
            .SetFontSize(12)
            .SetFontColor(headerColor))
            .SetBorder(Border.NO_BORDER));
        sellerTable.AddCell(new Cell().Add(new Paragraph("1234 Elm Street, London, UK\nChelsea, State, ZIP\nPhone: (123) 456-7890")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetFontColor(bodyColor))
            .SetBorder(Border.NO_BORDER));
        document.Add(sellerTable);

        document.Add(new Paragraph("\n"));

        // Buyer Details
        Table buyerTable = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();
        buyerTable.AddCell(new Cell().Add(new Paragraph("Buyer:")
            .SetFont(headerFont)
            .SetFontSize(12)
            .SetFontColor(headerColor))
            .SetBorder(Border.NO_BORDER));
        buyerTable.AddCell(new Cell().Add(new Paragraph($"Name: {user.Username}\nFullName: {firstName} {lastName}\nAddress: {address}\nContact: {phone}\nEmail: {email}")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetFontColor(bodyColor))
            .SetBorder(Border.NO_BORDER));
        document.Add(buyerTable);

        document.Add(new Paragraph("\n"));

        // Car Details Table
        Table carTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 5, 2 })).UseAllAvailableWidth();
        carTable.SetMarginBottom(10);

        carTable.AddCell(new Cell().Add(new Paragraph("Lot Number"))
            .SetFont(headerFont)
            .SetFontSize(10)
            .SetBackgroundColor(headerColor)
            .SetFontColor(ColorConstants.WHITE)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));
        carTable.AddCell(new Cell().Add(new Paragraph("Car Details"))
            .SetFont(headerFont)
            .SetFontSize(10)
            .SetBackgroundColor(headerColor)
            .SetFontColor(ColorConstants.WHITE)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));
        carTable.AddCell(new Cell().Add(new Paragraph("Price"))
            .SetFont(headerFont)
            .SetFontSize(10)
            .SetBackgroundColor(headerColor)
            .SetFontColor(ColorConstants.WHITE)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));

        carTable.AddCell(new Cell().Add(new Paragraph(car.LotNumber))
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));
        carTable.AddCell(new Cell().Add(new Paragraph($"{car.Make} {car.Model} ({car.Year})"))
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));
        carTable.AddCell(new Cell().Add(new Paragraph($"${car.Price:F2}"))
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 1)));

        document.Add(carTable);

        // Payment Information
        document.Add(new Paragraph("Payment Information:")
            .SetFont(headerFont)
            .SetFontSize(12)
            .SetFontColor(headerColor));
        document.Add(new Paragraph("Payment Method: Online")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetFontColor(bodyColor));
        document.Add(new Paragraph($"Card Number: {cardNumber}")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetFontColor(bodyColor));
        document.Add(new Paragraph($"Total Amount Due: ${car.Price:F2}")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetFontColor(bodyColor));

        document.Add(new Paragraph("\n"));

        // Footer and Signature Line
        document.Add(new Paragraph("Thank you for your business!")
            .SetFont(headerFont)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(headerColor));
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("Signature: __________________")
            .SetFont(bodyFont)
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontColor(bodyColor));
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("Please make all checks payable to Kenny Car Dealership.")
            .SetFont(bodyFont)
            .SetFontSize(8)
            .SetFontColor(bodyColor));
        document.Add(new Paragraph("If you have any questions concerning this invoice, contact our office at (123) 456-7890.")
            .SetFont(bodyFont)
            .SetFontSize(8)
            .SetFontColor(bodyColor));
    }
}
