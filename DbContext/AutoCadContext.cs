using Microsoft.EntityFrameworkCore;
using AutoCADApi.Models;

public class AutoCadContext : DbContext
{
    public AutoCadContext(DbContextOptions<AutoCadContext> options) : base(options) { }

    public DbSet<ImageFile> ImageFiles { get; set; }
    public DbSet<AutoCADFile> AutoCADFiles { get; set; }
    public DbSet<Pin> Pins { get; set; }
    public DbSet<ModalContent> ModalContents { get; set; }
}
