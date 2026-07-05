using dotnet_store.Exceptions;
using dotnet_store.Models;

namespace dotnet_store.Services;

public interface IKategoriService
{
    Task Create(string kategoriAdi, string url);
    Task Edit(int id, string kategoriAdi, string url);
    Task Delete(int id);
}

public class KategoriService : IKategoriService
{
    private readonly DataContext _context;

    public KategoriService(DataContext context)
    {
        _context = context;
    }

    public async Task Create(string kategoriAdi, string url)
    {
        var entity = new Kategori() { KategoriAdi = kategoriAdi, Url = url };

        await _context.Kategoriler.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _context.Kategoriler.FindAsync(id);

        if (entity == null)
            throw new NotFoundException($"Id: {id} olan kategori bulunamadı");

        _context.Kategoriler.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Edit(int id, string kategoriAdi, string url)
    {
        var entity = _context.Kategoriler.FirstOrDefault(i => i.Id == id);

        if (entity == null)
            throw new NotFoundException($"Id: {id} olan kategori bulunamadı");

        entity.KategoriAdi = kategoriAdi;
        entity.Url = url;

        await _context.SaveChangesAsync();
    }
}
