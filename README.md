# Ürün Katalog Projesi ✈️

### Üye Ol Endpoint Detayları
• Kullanıcılar sisteme uye olabilmeli. Kayit isleminde alinan bilgiler eksiksiz olmali ve validate edilmeli. Email bilgisi gecerli olmali.
• Kayit sirasinda kullanici sifresi sifrelenmis sekilde databasede saklanmali. 
• Ayni sifreye sahip kullanicilarin hashelenmis sifreleri mutlaka farkli olmali. (Tuzlama).
• Sifreler geri cozulemeyecek sekilde sifrelenip saklanmali.
• Email valid olmalı.
• En az 8 ve en fazla 20 karakter uzunluğunda bir password girilmeli.
• İşlem başarısız ise kullanıcıya tasarıma göre hata mesajı gösterilmeli.
• İşlem başarılı ise API'den basarili mesaji gonderilmeli ve Hosgeldiniz Email i gonderilmeli.

### Üye Girişi Endpoint Detayları
• Kullanıcılar burden üye girişi yapabilmeli.
• Email ve Password alanları zorunlu alanlar olmali. Bos yada gecersiz gonderilirse uyari verilmeli.
• Email ve Password alanlarının validasyonu yapılmalı.
• Email valid olmalı ve en az 8 ve en fazla 20 karakter uzunluğunda bir password girilmeli.
• İşlem başarısız ise kullanıcıya hata mesajı gösterilmeli.
• İşlem başarılı ise API'de JWT token uretilmeli ve tüm authantication gerektiren requestlerde header'a Bearer token olarak eklenmeli.
• 3 kez parolanin yanlis girilmesi durumunda hesabi bloke ediniz ve kullaniciya bilgilendirme maili gonderiniz.

### Kategori Endpoint Detayları
• Tüm kategoriler listelenmeli
• Kullanıcı kategori id ile api call yaptiginda kategori altindaki ürünler kategoriye göre filtrelenmeli, default olarak tüm ürünler çekilmeli.
• Yeni kategori eklenebilmeli veya mevcut olan guncellenebilmeli. 

### Ürün Endpoint Detayları
• Teklif Ver apisi üründen gelen data içerisindeki isOfferable alanına gore control edilmeli.
• isOfferable durumunun saglanmadigi takdirde teklif verilememeli. 
• Teklif Ver apisi ile kullanıcı kendisi teklif girebilmeli. Teklif girme alanı number olmalı ve buraya validasyon eklenmeli.
• ayrica Teklif degeri yuzdelik olarak api tarafına yollanabilmeli (offeredPrice), mesela, 100₺ olan ürün için %40 değeri seçilirse, 40₺ teklif yapılabilmeli.
• Eğer bir kullanıcı bir ürüne teklif verdiyse, o ürünün icin teklifini geri cekebilmeli. Verdigi teklif yoksa kullanicilar bilgilendirilmeli. 
• Kullanıcı teklif yapmadan bir ürünü direk satın alabilir. Kullanıcı ürünü satın alınca, ilgili ürün datası içerisindeki isSold alanının değeri guncellenmeli.

### Hesabım Endpoint Detayları
• Kullanicinin yaptigi offer lar listelenmeli. 
• Kullanicinin urunleri icin aldigi offer lar listelenmeli. 
• Alınan tekliflere Onayla ve Reddet ile cevap verilebilmeli.
• Verilen teklif onaylandığında satin alma icin uygun duruma getirilmeli. 
• Ürün detay daki gibi Satın Al tetiklenince statu guncellenmeli. Satın Al'a tetiklenince Teklif Verdiklerim listesindeki ürünün durumu güncellenmeli.

### Ürün Ekleme Endpoint Detayları
** İlgili validasyonlar eklenmeli:
• Ürün Adı alanı maksimum 100 karakter uzunluğunda olmalı ve zorunlu bir alan olmalı.
• Açıklama alanı maksimum 500 karakter uzunluğunda olmalı ve zorunlu bir alan olmalı.
• Kategori alanı ilgili endpointten çekilen kategorileri listelemeli ve en fazla bir kategori seçilebilmeli. Bu alan zorunlu bir alan olmalı.
• Renk alanı ilgili endpointten çekilen renkleri listelemeli ve en fazla bir renk seçilebilmeli. Bu alan zorunlu bir alan olmamalı.
• Marka alanı ilgili endpointten çekilen markaları listelemeli ve en fazla bir marka seçilebilmeli. Bu alan zorunlu bir alan olmamalı.
• Kullanım Durumu alanı ilgili endpointten çekilen kullanım durumlarını listelemeli ve en fazla bir kullanım durumu seçilebilmeli. Bu alan zorunlu bir alan olmalı
• Ürün Görseli alanından en fazla bir ürün görseli eklenmeli. Eklenen ürün görseli istenildiği zaman silinebilmeli. Bu alan zorunlu bir alan olmalı.
• Fiyat alanı number olmalı ve zorunlu bir alan olmalı.
• Teklif Opsiyonu alanı boolean bir değer olmalı ve default olarak false olmalı.

### Email Servisi
• Email gonderme islemlerini Sync olarak gonderecek bir tasarim yapmayiniz. 
• Email ler bir kuyruk tablosunda toplanmali ve bir process ordan email gonderimi yapmali. 
• Database,kafka, rabbitmq vs uzerinde kuyruklama islemi yapabilirsiniz. 
• Hangfire gibi servisler kullanarak da yapabilirsiniz.
• Kuyruga gelen her email in en gec 2sn icerisinde process edilmeli. 
• Gonderilen email ler in status durumunu guncelleyiniz. 
• Try count ile basarisiz olmasi durumunda terkar gondermesini saglayiniz. 
• 5 kez deneyip basarisiz olan kayitlari Farkli bir statuye cekerek guncelleyiniz.
• Smtp entegrasyonu yaparak mail gonderimini saglayiniz. 
• Smtp hizmetinin calisir sekilde oldugundan emin olunuz.

#### Authorize kısmına tokenımızın başına Bearer ve bir boşluk bırakarak giriyoruz. Register ve Login endpointleri haricinde bütün endpointler authorize istiyor 🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/authorize.JPG)

#### Üye olma 🌺 Bilgilerimizi validasyona uygun girip execute ettiğimizde access token'ımız ve userid'miz dönecektir. 
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/register.JPG)
#### Üye olduktan saniyeler içinde Hoşgeldiniz maili gelir;
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/ho%C5%9Fgeldiniz%20maili.JPG)

#### Giriş yapma 🌺 Bilgilerimizi doğru girdiysek 'Giriş başarılı!' mesajı döner. 3 kez yanlış girdiysek hesabımız bloke olup silinir ve bize bilgilendirme maili gelir.
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/login%20bloke.JPG)
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/bloke%20maili.JPG)

#### Hangfire dashboard'da mail atmak için kullandığımız Job'lar başarılı kısmında görünüyor; 
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/hangfire.JPG)


#### Tüm kategoriyi listeleme 🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/T%C3%BCmkategorilisteleme.JPG)

#### Kategori id'si ile listeleme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/%C4%B0dyeg%C3%B6rekategori%20listeleme.JPG)

#### Kategori create etme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kategori%20create.JPG)

#### Kategori update etme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kategori%20update.JPG)

#### Kategori silme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kategori%20silme.JPG)

#### Kullanıcının yaptığı teklifleri listeleme. 🌺 ‼️ username'e gerek yoktu ancak bir controller'da 2 Get metodu olduğu için çalışması için parametre ekledim. ‼️
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kullan%C4%B1c%C4%B1n%C4%B1n%20yapt%C4%B1%C4%9F%C4%B1%20teklifler.JPG)

#### Kullanıcının aldığı teklifleri listeleme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kullan%C4%B1c%C4%B1n%C4%B1n%20ald%C4%B1%C4%9F%C4%B1%20teklifler.JPG)

#### Verilen teklifi kabul etme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/verilen%20teklifi%20kabul%20etme.JPG)

#### Teklif yapma 🌺 Body'nin içindeki isOfferPercentage'ı true yapıp offeredPrice'ı da yüzdelik verirsek (örneğin 20 => %20) teklifler tablosuna fiyat üzerinden yüzde hesabı yaparak düşer. Aşağıda görüldüğü gibi teklifi yapılan ürün %30'u düşülerek tabloya kaydolmuştur.
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/y%C3%BCzdelik%20teklif%20verme.JPG)

#### Eğer Body'nin içindeki isOfferPercentage'ı false yapıp offeredPrice'ı verirsek teklifler tablosuna ne fiyat teklif ettiysek o düşer.
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/y%C3%BCzdeliksiz%20teklif%20verme.JPG)

#### Verilen teklifin geri çekilmesi🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/teklif%20geri%20%C3%A7ekme.JPG)

#### Eğer kullanıcı kendine ait olmayan teklifi silmek isterse şu şekilde hata verecektir; 
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/kendisine%20ait%20olmayan%20teklifi%20silme.JPG)

#### Ürün ekleme🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/%C3%BCr%C3%BCn%20ekleme.JPG)
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/%C3%BCr%C3%BCn%20ekleme%202.JPG)
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/%C3%BCr%C3%BCn%20ekleme%203.JPG)

#### Ürün satın alma🌺
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/%C3%BCr%C3%BCn%20sat%C4%B1n%20alma.JPG)

#### Eğer satın alınmış bir ürünü satın almaya kalkarsak şöyle bir hata alırız;
![alt text](https://github.com/160-Sodexo-NET-Bootcamp/bitirmeprojesi-ipekserttas99/blob/main/MezuniyetProjesi/readme%20photos/sat%C4%B1n%20al%C4%B1nm%C4%B1%C5%9F%20%C3%BCr%C3%BCn%C3%BC%20sat%C4%B1n%20almaya%20kalkarsak.JPG)
