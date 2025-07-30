using System.Threading.Tasks;
using ChienVHShopOnline.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChienVHShopOnline.Reports;

public class OrderReportDocument : IDocument
{
    private readonly IEnumerable<Order> _orders;
    public OrderReportDocument(IEnumerable<Order> orders)
    {
        _orders = orders;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header().Text("Order Listing Report")
                .FontSize(20)
                .Bold()
                .AlignCenter();

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(); // Order Date
                    columns.RelativeColumn(); // Payment Type
                    columns.RelativeColumn(); // Name
                    columns.RelativeColumn(); // Phone
                    columns.RelativeColumn(); // Email
                    columns.RelativeColumn(); // Address
                    columns.RelativeColumn(); // Status
                });

                // Table Header
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Order Date").Bold();
                    header.Cell().Element(CellStyle).Text("Payment Type").Bold();
                    header.Cell().Element(CellStyle).Text("Name").Bold();
                    header.Cell().Element(CellStyle).Text("Phone").Bold();
                    header.Cell().Element(CellStyle).Text("Email").Bold();
                    header.Cell().Element(CellStyle).Text("Address").Bold();
                    header.Cell().Element(CellStyle).Text("Status").Bold();
                });

                // Table Body
                foreach (var order in _orders)
                {
                    table.Cell().Element(CellStyle).Text(order.OrderDate.ToString()); // nicely formatted date-time
                    table.Cell().Element(CellStyle).Text(order.PaymentType);
                    table.Cell().Element(CellStyle).Text(order.CustomerName);
                    table.Cell().Element(CellStyle).Text(order.CustomerPhone);
                    table.Cell().Element(CellStyle).Text(order.CustomerEmail);
                    table.Cell().Element(CellStyle).Text(order.CustomerAddress);
                    table.Cell().Element(CellStyle).Text(order.Status);
                }
            });

            page.Footer().AlignRight().Text(text =>
            {
                text.Span("Generated on: ").SemiBold();
                text.Span(DateTime.Now.ToString("g"));
            });
        });

        // Local helper for styling cells
        IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(5).PaddingHorizontal(3).BorderBottom(1).AlignMiddle();
    }
}