using Microsoft.AspNetCore.Mvc;

namespace KuaforSalonuProje.Controllers
{
    public class AIController : Controller
    {
        // Yapay zeka yardımı al sayfası
        [HttpGet]
        public IActionResult Consult()
        {
            return View();
        }

        // Kullanıcının yüklediği fotoğrafı işlemek için method
        [HttpPost]
        public async Task<IActionResult> ProcessPhoto(IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                // Fotoğrafı geçici bir dosyaya kaydediyoruz
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Fotoğrafı AI API'sine gönderme işlemi (önceki adımda gösterildiği gibi)
                var recommendations = await GetHairRecommendations(filePath);

                // Önerileri ViewBag ile View'a gönderiyoruz
                ViewBag.Recommendations = recommendations;

                return View("Consult");
            }
            else
            {
                return View("Error"); // Fotoğraf alınamazsa hata sayfası
            }
        }

        // Yapay zeka API'sine fotoğrafı gönderme ve öneri alma metodu
        private async Task<List<string>> GetHairRecommendations(string imagePath)
        {
            var recommendations = new List<string>();

            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imagePath));
                content.Add(fileContent, "photo", Path.GetFileName(imagePath));

                var response = await client.PostAsync("YAPAY_ZEKA_API_URL", content); // API URL'sini buraya ekleyin
                if (response.IsSuccessStatusCode)
                {
                    var apiResult = await response.Content.ReadAsStringAsync();

                    // Burada API'den gelen sonuçları işleyin
                    recommendations.Add("Saç Kesimi Önerisi: Kısa Bob");
                    recommendations.Add("Saç Rengi Önerisi: Koyu Kahverengi");
                }
                else
                {
                    recommendations.Add("Yapay zeka API'si ile bağlantı kurulamadı.");
                }
            }

            return recommendations;
        }
    }
}
