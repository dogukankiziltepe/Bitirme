# 🎓 Bitirme Projesi - .NET 8 + Docker + Katmanlı Mimari

Bu proje, mezuniyet çalışması olarak geliştirilmiş, Docker destekli, .NET 8 ile yazılmış bir web API mimarisidir. Temiz kod, test edilebilirlik ve sürdürülebilirlik esas alınarak modern bir katmanlı yapı kurulmuştur.

---

## 📦 Teknolojiler

- .NET 8 Web API
- Entity Framework Core
- Katmanlı Mimari (API, BLL, DAL)
- PostgreSQL veya MSSQL
- Docker ve Docker Compose
- Swagger
- JWT Authentication

---

## 🧱 Katmanlar

- **Bitirme.API**: Giriş noktası, controller ve middleware yapıları burada.
- **Bitirme.BLL**: İş kuralları, DTO’lar, servis arayüzleri ve validasyonlar.
- **Bitirme.DAL**: Entity’ler, interface tanımları, enumlar,DbContext, repository implementasyonları, migration işlemleri.

---

## 🚀 Projeyi Çalıştırma

### Gereksinimler

- .NET 8 SDK
- Docker
- PostgreSQL ya da MSSQL (Docker ile otomatik başlar)
- Visual Studio veya VS Code

### Adımlar

```bash
git clone https://github.com/dogukankiziltepe/Bitirme.git
cd Bitirme
docker-compose up --build
