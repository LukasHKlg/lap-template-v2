using OnlineShop.ApiService.Data;
using OnlineShop.ApiService.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OnlineShop.ApiService.Services
{
    public class GeneratePDFInvoice : IDocument
    {
        public SalesOrder Order { get; set; }
        public List<CartItem> CartItems { get; set; }
        private Address address;
        private string Title;


        public GeneratePDFInvoice(SalesOrder order, List<CartItem> cartItems)
        {
            Order = order;
            CartItems = cartItems;
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
                        Title = "Shipping Address";
                        address = Order.ShippingAddress;
                        row.RelativeItem().Element(ComposeAddress);

                        row.ConstantItem(50);

                        Title = "Billing Address";
                        address = Order.BillingAddress;
                        row.RelativeItem().Element(ComposeAddress);
                    });

                    column.Spacing(30);

                    column.Item().Element(ComposeTable);

                    column.Item().AlignRight().Text($"Grand total: {Order.Cart.TotalPrice}€").FontSize(14);

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

                    foreach (var item in CartItems)
                    {
                        table.Cell().Element(CellStyle).Text(item.Product.Id + "");
                        table.Cell().Element(CellStyle).Text(item.Product.Name);
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Product.Price}€");
                        table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity + "");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}€");

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

            void ComposeAddress(IContainer container)
            {
                container.Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                    column.Item().Text(Order.Customer.FirstName + " " + Order.Customer.LastName);
                    column.Item().Text(address.Country);
                    column.Item().Text(address.Street + " " + address.HouseNum);
                    column.Item().Text($"{address.HouseNum} {address.City}");
                    column.Item().Text(Order.Customer.ApplicationUser?.Email ?? "no email");
                    column.Item().Text(Order.Customer.PhoneNumber);
                });
            }
        }
    }
}
