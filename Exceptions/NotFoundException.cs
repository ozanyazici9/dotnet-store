namespace dotnet_store.Exceptions;

/// <summary>
/// Aranan bir kayıt (ürün, sepet öğesi, sipariş vb.) veritabanında bulunamadığında
/// fırlatılır. Domain'e (sepet/ürün/sipariş) özel değildir - "bulunamadı" durumunun
/// genel karşılığıdır, her serviste kullanılabilir.
///
/// Örnek:
///   throw new NotFoundException($"Id: {urunId} olan ürün bulunamadı.");
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message) { }
}
