using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuaforSalonuProje.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string KullaniciAdi {  get; set; }
        public string Sifre {  get; set; }
    }
}
