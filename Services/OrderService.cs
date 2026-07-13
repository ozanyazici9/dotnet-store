using dotnet_store.Data;
using dotnet_store.Exceptions;
using dotnet_store.Models;
using dotnet_store.Services.Results;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Services;

public interface IOrderService
{
    Task<ServiceResult<Order>> Checkout(OrderCreateModel model, string userName, Cart cart);
    Task<ServiceResult<OrderGetModel>> Details(int orderId);
    Task<ServiceResult<List<OrderGetModel>>> OrderList(string userName);
}

public class OrderService : IOrderService
{
    private readonly ICartService _cartService;
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public OrderService(ICartService cartService, IConfiguration configuration, DataContext context)
    {
        _cartService = cartService;
        _configuration = configuration;
        _context = context;
    }

    public async Task<ServiceResult<Order>> Checkout(
        OrderCreateModel model,
        string userName,
        Cart cart
    )
    {
        var order = new Order
        {
            UserName = userName,
            AdSoyad = model.AdSoyad,
            Sehir = model.Sehir,
            AdresSatiri = model.AdresSatiri,
            PostaKodu = model.PostaKodu,
            Telefon = model.Telefon,
            SiparisNotu = model.SiparisNotu!,
            SiparisTarihi = DateTime.Now,
            ToplamFiyat = cart.Toplam(),
            OrderItems = cart
                .CartItems.Select(i => new Data.OrderItem
                {
                    Urun = i.Urun,
                    UrunId = i.UrunId,
                    Miktar = i.Miktar,
                    Fiyat = i.Urun.Fiyat,
                })
                .ToList(),
        };

        var payment = await ProcessPayment(model, cart);

        if (payment.Status == "success")
        {
            await _context.Orders.AddAsync(order);
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return ServiceResult<Order>.Success(order);
        }

        return ServiceResult<Order>.Fail("Ödeme İşlemi Başarısız");
    }

    public async Task<ServiceResult<OrderGetModel>> Details(int orderId)
    {
        var order = await _context
            .Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Urun)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new NotFoundException("Sipariş Bulunamadı");

        var model = new OrderGetModel
        {
            Id = order.Id,
            SiparisTarihi = order.SiparisTarihi,
            AdresSatiri = order.AdresSatiri,
            PostaKodu = order.PostaKodu,
            Sehir = order.Sehir,
            SiparisNotu = order.SiparisNotu,
            AdSoyad = order.AdSoyad,
            Telefon = order.Telefon,
            ToplamFiyat = order.ToplamFiyat,
            UserName = order.UserName,
            OrderItems = order
                .OrderItems.Select(oi => new OrderItemModel
                {
                    Id = oi.Id,
                    UrunId = oi.UrunId,
                    Miktar = oi.Miktar,
                    Fiyat = oi.Fiyat,
                    Urun = new UrunGetModel
                    {
                        UrunAdi = oi.Urun.UrunAdi,
                        Resim = oi.Urun.Resim,
                        Aciklama = oi.Urun.Aciklama,
                        Fiyat = oi.Urun.Fiyat,
                        KategoriAdi = oi.Urun.Kategori.KategoriAdi,
                    },
                })
                .ToList(),
            AraToplam = order.AraToplam(), // Urun artık yüklü, doğru çalışır
            Vergi = order.Vergi(),
        };

        return ServiceResult<OrderGetModel>.Success(model);
    }

    public async Task<ServiceResult<List<OrderGetModel>>> OrderList(string userName)
    {
        if (userName == null)
            throw new NotFoundException("Kullanıcı Bulunamadı");

        var orders = await _context
            .Orders.Where(i => i.UserName == userName)
            .Include(i => i.OrderItems)
                .ThenInclude(i => i.Urun)
            .ToListAsync();

        var model = orders
            .Select(i => new OrderGetModel
            {
                Id = i.Id,
                AdSoyad = i.AdSoyad,
                SiparisTarihi = i.SiparisTarihi,
                AdresSatiri = i.AdresSatiri,
                Sehir = i.Sehir,
                PostaKodu = i.PostaKodu,
                ToplamFiyat = i.ToplamFiyat,
                Telefon = i.Telefon,
                OrderItems = i
                    .OrderItems.Select(i => new OrderItemModel
                    {
                        Fiyat = i.Fiyat,
                        Miktar = i.Miktar,
                        Urun = new UrunGetModel
                        {
                            UrunAdi = i.Urun.UrunAdi,
                            Resim = i.Urun.Resim,
                            Aciklama = i.Urun.Aciklama,
                            Fiyat = i.Urun.Fiyat,
                            KategoriAdi = i.Urun.Kategori.KategoriAdi,
                        },
                        UrunId = i.UrunId,
                    })
                    .ToList(),
                AraToplam = i.AraToplam(),
                Vergi = i.Vergi(),
            })
            .ToList();

        return ServiceResult<List<OrderGetModel>>.Success(model);
    }

    private async Task<Payment> ProcessPayment(OrderCreateModel model, Cart cart)
    {
        Options options = new Options();
        options.ApiKey = _configuration["PaymentAPI:APIKey"];
        options.SecretKey = _configuration["PaymentAPI:SecretKey"];
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = cart.AraToplam().ToString();
        request.PaidPrice = cart.AraToplam().ToString();
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model.CartName;
        paymentCard.CardNumber = model.CartNumber;
        paymentCard.ExpireMonth = model.CartExpirationMonth;
        paymentCard.ExpireYear = model.CartExpirationYear;
        paymentCard.Cvc = model.CartCVV;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = model.AdSoyad;
        buyer.Surname = "Doe";
        buyer.GsmNumber = model.Telefon;
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = model.Sehir;
        buyer.Country = "Turkey";
        buyer.ZipCode = model.PostaKodu;
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = model.AdSoyad;
        shippingAddress.City = model.Sehir;
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = model.AdresSatiri;
        shippingAddress.ZipCode = model.PostaKodu;
        request.ShippingAddress = shippingAddress;

        Address billingAddress = new Address();
        billingAddress.ContactName = model.AdSoyad;
        billingAddress.City = model.Sehir;
        billingAddress.Country = "Turkey";
        billingAddress.Description = model.AdresSatiri;
        billingAddress.ZipCode = model.PostaKodu;
        request.BillingAddress = billingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();

        foreach (var item in cart.CartItems)
        {
            BasketItem basketItem = new BasketItem();
            basketItem.Id = item.CartItemId.ToString();
            basketItem.Name = item.Urun.UrunAdi;
            basketItem.Category1 = "Phone";
            basketItem.Category2 = "Accessories";
            basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            basketItem.Price = item.Urun.Fiyat.ToString();

            basketItems.Add(basketItem);
        }

        request.BasketItems = basketItems;

        return await Payment.Create(request, options);
    }
}
