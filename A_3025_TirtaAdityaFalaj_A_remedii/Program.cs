using System;
class Sekolah
{
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string id, string nama, int jumlah)
    {
        idSekolah = id;
        namaSekolah = nama;
        jumlahPaketDiterima = jumlah;
    }

    public void TampilInfo()
    {
        Console.WriteLine("ID Sekolah           : " + idSekolah);
        Console.WriteLine("Nama Sekolah         : " + namaSekolah);
        Console.WriteLine("Jumlah Paket Diterima: " + jumlahPaketDiterima);
    }
}

abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    public int StokDapur
    {
        get { return stokDapur; }
    }

    public PaketMBG(string id, string nama, int stok)
    {
        if (stok < 0)
            throw new ArgumentException("Stok tidak boleh negatif!");

        idPaket = id;
        namaPaket = nama;
        stokDapur = stok;
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Jumlah pengurangan harus lebih dari 0!");
            return false;
        }

        if (jumlah > stokDapur)
        {
            Console.WriteLine("Stok dapur tidak mencukupi!");
            return false;
        }

        stokDapur -= jumlah;
        return true;
    }

    public abstract int HitungBiaya(int jumlah);
}

class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok)
        : base(id, nama, stok)
    {
    }

    public override int HitungBiaya(int jumlah)
    {
        if (jumlah < 0)
            throw new ArgumentException("Jumlah tidak boleh negatif!");

        return checked(jumlah * 15000);
    }
}

class PaketKhusus : PaketMBG
{
    private string jenisDiet;

    public string JenisDiet
    {
        get { return jenisDiet; }
        set { jenisDiet = value; }
    }

    public PaketKhusus(string id, string nama, int stok, string diet)
        : base(id, nama, stok)
    {
        jenisDiet = diet;
    }

    public override int HitungBiaya(int jumlah)
    {
        if (jumlah < 0)
            throw new ArgumentException("Jumlah tidak boleh negatif!");

        return checked(jumlah * 20000);
    }
}

interface DapatDidistribusikan
{
    void KirimPaket();
}

class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(Sekolah sekolah, PaketMBG paketMakanan, int jumlahKirim)
    {
        if (sekolah == null || paketMakanan == null)
            throw new ArgumentNullException("Data sekolah atau paket tidak boleh kosong!");

        sekolahTujuan = sekolah;
        paket = paketMakanan;
        jumlah = jumlahKirim;
    }

    public void KirimPaket()
    {
        Console.WriteLine("\n--- PROSES DISTRIBUSI MBG ---");

        if (jumlah <= 0)
        {
            Console.WriteLine("Distribusi gagal! Jumlah paket harus lebih dari 0.");
            return;
        }

        if (jumlah > paket.StokDapur)
        {
            Console.WriteLine("Distribusi gagal! Stok dapur tidak mencukupi.");
            return;
        }

        if (paket.KurangiStok(jumlah))
        {
            sekolahTujuan.jumlahPaketDiterima += jumlah;

            Console.WriteLine("Distribusi MBG berhasil! Sebanyak "
                + jumlah + " paket makanan telah dikirim ke sekolah "
                + sekolahTujuan.namaSekolah + ".");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEM MANAJEMEN DISTRIBUSI MBG ===");

        Sekolah sekolah1 = new Sekolah("S001", "SMA Negeri 1 Jember", 0);
        Sekolah sekolah2 = new Sekolah("S002", "SMP Negeri 2 Jember", 0);

        PaketReguler reguler = new PaketReguler("P001", "Paket Reguler", 500);
        PaketKhusus khusus = new PaketKhusus("P002", "Paket Khusus", 300, "Rendah Gula");

        Console.WriteLine("\n=== DATA SEKOLAH ===");

        sekolah1.TampilInfo();

        Console.WriteLine();

        sekolah2.TampilInfo();

        Console.WriteLine("\n=== DATA PAKET MBG ===");

        Console.WriteLine("Nama Paket : " + reguler.NamaPaket);
        Console.WriteLine("Stok Dapur : " + reguler.StokDapur);

        Console.WriteLine("\nNama Paket : " + khusus.NamaPaket);
        Console.WriteLine("Jenis Diet : " + khusus.JenisDiet);
        Console.WriteLine("Stok Dapur : " + khusus.StokDapur);

        Console.WriteLine("\n=== PERHITUNGAN BIAYA ===");

        PaketMBG paket1 = reguler;
        PaketMBG paket2 = khusus;

        Console.WriteLine("Biaya 10 Paket Reguler : Rp" + paket1.HitungBiaya(10));
        Console.WriteLine("Biaya 10 Paket Khusus  : Rp" + paket2.HitungBiaya(10));

        DistribusiMBG distribusi1 = new DistribusiMBG(sekolah1, reguler, 100);
        distribusi1.KirimPaket();

        DistribusiMBG distribusi2 = new DistribusiMBG(sekolah2, khusus, 50);
        distribusi2.KirimPaket();

        DistribusiMBG distribusi3 = new DistribusiMBG(sekolah1, reguler, 1000);
        distribusi3.KirimPaket();

        Console.WriteLine("\n=== LAPORAN AKHIR DISTRIBUSI ===");

        Console.WriteLine("\nData Sekolah 1:");
        sekolah1.TampilInfo();

        Console.WriteLine("\nData Sekolah 2:");
        sekolah2.TampilInfo();

        Console.WriteLine("\nSisa Stok Paket Reguler : " + reguler.StokDapur);
        Console.WriteLine("Sisa Stok Paket Khusus  : " + khusus.StokDapur);

        Console.WriteLine("\n=== PROGRAM SELESAI ===");
    }
}