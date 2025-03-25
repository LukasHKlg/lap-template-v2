using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;

namespace OnlineShop.ApiService.Services
{
    public class GeneratePDFInvoice : IDocument
    {
        public SalesOrder Order { get; set; }
        public List<OrderDetail> OrderItems { get; set; }


        public GeneratePDFInvoice(SalesOrder order, List<OrderDetail> orderitems)
        {
            Order = order;
            OrderItems = orderitems;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;


        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });


            void ComposeHeader(IContainer container)
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item()
                            .Text($"Invoice #{Order.Id}")
                            .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                        column.Item().Text(text =>
                        {
                            text.Span("Date: ").SemiBold();
                            text.Span($"{Order.OrderDate:d}");
                        });
                    });

                    row.ConstantItem(100).Height(50).Placeholder();
                });
            }

            void ComposeContent(IContainer container)
            {
                container.PaddingVertical(40).Column(column =>
                {
                    column.Spacing(5);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Element(ComposeBillingAddress);

                        row.ConstantItem(50);

                        row.RelativeItem().Element(ComposeShippingAddress);
                    });

                    column.Spacing(30);

                    column.Item().Element(ComposeTable);

                    var totalPrice = OrderItems.Sum(x => x.UnitPrice * x.Quantity);
                    column.Item().AlignRight().Text($"Grand total: {totalPrice}€").FontSize(14);

                    //if (!string.IsNullOrWhiteSpace(Model.Comments))
                    //    column.Item().PaddingTop(25).Element(ComposeComments);
                });
            }

            void ComposeTable(IContainer container)
            {
                container.Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#");
                        header.Cell().Element(CellStyle).Text("Product");
                        header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                        header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    foreach (var item in OrderItems)
                    {
                        table.Cell().Element(CellStyle).Text(item.Product.Id + "");
                        table.Cell().Element(CellStyle).Text(item.Product.Name);
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice}€");
                        table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity + "");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice * item.Quantity}€");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }
                    }
                });
            }

            void ComposeComments(IContainer container)
            {
                container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("Comments").FontSize(14);
                    //column.Item().Text(Model.Comments);
                });
            }

            void ComposeShippingAddress(IContainer container)
            {
                container.Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Shipping Address").SemiBold();

                    column.Item().Text(Order.ShipName);
                    column.Item().Text(Order.ShipCountry);
                    column.Item().Text(Order.ShipStreet + " " + Order.ShipHouseNum);
                    column.Item().Text($"{Order.ShipHouseNum} {Order.ShipCity}");
                    column.Item().Text(Order.Customer.ApplicationUser?.Email ?? "no email");
                    column.Item().Text(Order.Customer.PhoneNumber);
                });
            }
            void ComposeBillingAddress(IContainer container)
            {
                container.Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Billing Address").SemiBold();

                    column.Item().Text(Order.BillingName);
                    column.Item().Text(Order.BillingCountry);
                    column.Item().Text(Order.BillingStreet + " " + Order.BillingHouseNum);
                    column.Item().Text($"{Order.BillingHouseNum} {Order.BillingCity}");
                    column.Item().Text(Order.Customer.ApplicationUser?.Email ?? "no email");
                    column.Item().Text(Order.Customer.PhoneNumber);
                });
            }
        }
    }
}
