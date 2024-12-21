# PrinterAnaliz.App

PrinterAnaliz.App, endüstriyel yazıcıların loglarını kaydetmek ve bu logları analiz etmek için geliştirilmiş bir **micro servis** projesidir. Bu proje, deneme amaçlı olarak geliştirilmiştir ve .NET Core kullanarak neler yapabileceğimi test etmek üzere hazırlanmıştır. İlerideki projelerimde kendime referans olması amacıyla tasarlanmıştır.

## Amaç

- **.NET Core** ve **MSSQL** ile bir micro servis projesi oluşturmak.
- **Stored Procedures** ile .NET Core entegrasyonunu göstermek.
- **Mapper** ve **Mediator** kullanımına dair örnekler sağlamak.
- **Swagger** içeriğinin nasıl özelleştirileceğine dair örnekler sunmak.

Bu proje, herhangi bir yerde yayımlanmamış olup, tamamen şahsi gelişim ve gelecekteki projelerime referans olması amacıyla tasarlanmıştır.

---

## Özellikler

- **Endüstriyel Yazıcı Loglama**: Yazıcılardan gelen logları MSSQL veritabanında kayıt altına alma.
- **Stored Procedures Kullanımı**: Veritabanı işlemleri için optimize edilmiş sorgular.
- **Mapper Kullanımı**: Nesneler arasındaki dönüşümleri basitleştirmek.
- **Mediator Yapısı**: CQRS (Command-Query Responsibility Segregation) deseni ile işlemleri yönetme.
- **Swagger Özelleştirme**: API dokümantasyonunun özelleştirilmesi.

---

## Kullanılan Teknolojiler

- **.NET Core**: Projenin ana iskeletini oluşturmak için kullanıldı.
- **MSSQL**: Veritabanı sistemi.
- **Mapper**: Nesne dönüştürme ve veri haritalama.
- **Mediator**: CQRS deseni.
- **Swagger**: API dokümantasyonu ve test.

---

## Kurulum

Bu projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1. **Proje dosyalarını klonlayın:**
   ```bash
   git clone https://github.com/msahyilmaz/PrinterAnaliz.App.git
   cd PrinterAnaliz.App
   ```

2. **Gerekli bağımlılıkları yükleyin:**
   ```bash
   dotnet restore
   ```

3. **MSSQL Bağlantısını Yapılandırın:**
   - `appsettings.json` dosyasında MSSQL bağlantı dizelerinizi (örneğin `ConnectionStrings:DefaultConnection`) din `ConnectionStrings:DefaultConnection`) d\u00fzenleyin.

4. **Veritabanı migrasyonlarını uygulayın:**
   ```bash
   dotnet ef database update
   ```

5. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```

6. **Swagger arayüzünü kullanarak API'yi test edin:**
   - Tarayıcınızda `https://localhost:{port}/swagger` adresine gidin.

---

## Kullanım

Proje tamamen bir deneme amacı taşıdığı için aşağıdaki senaryoları kapsar:

1. **Log Kaydetme**: Endüstriyel yazıcıdan gelen logların MSSQL veritabanında saklanması.
2. **Stored Procedure Kullanımı**: MSSQL tarafında belirli işlemler için hazırlanmış prosedürlerin çağrılması.
3. **Swagger Özelleştirme**: API dokümantasyonu nasıl özelleştirilir? Örnekler.

---

## Katkı

Bu proje şahsi gelişim için oluşturulmuştur ve herhangi bir yerde yayınlanmamıştır. Ancak ilerde benzer projeler yapacak olanlara fikir vermek için bir temel olarak kullanılabilir.

---

## Lisans

Bu proje özel olarak geliştirildiği için herhangi bir lisans belirtilmemiştir.
