using Microsoft.AspNetCore.Identity;

namespace KuaforSalonuProje.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ek kullanıcı özellikleri ekleyebilirsiniz (isteğe bağlı)
        public string FullName { get; set; } // Örnek ek özellik
    }
}
