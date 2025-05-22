using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private static List<Kerusakan> kerusakanList = new List<Kerusakan>
        {
            new Kerusakan { Merek = "Toyota", PlatNomor = "AB 123 CD", Kendala = "Mesin Overheating" },
            new Kerusakan { Merek = "Honda", PlatNomor = "EF 456 GH", Kendala = "AC Rusak" }
        };

        // API untuk mendapatkan semua kerusakan
        [HttpGet("getKerusakan")]
        public IActionResult GetKerusakan()
        {
            return Ok(kerusakanList);
        }

        // API untuk memperbarui kerusakan
        [HttpPut("updateKerusakan/{platNomor}")]
        public IActionResult UpdateKerusakan(string platNomor, [FromBody] Kerusakan updatedKerusakan)
        {
            var kerusakan = kerusakanList.Find(k => k.PlatNomor == platNomor);
            if (kerusakan == null)
                return NotFound("Kerusakan tidak ditemukan!");

            kerusakan.Kendala = updatedKerusakan.Kendala;
            return Ok("Kerusakan berhasil diperbarui!");
        }

        // API untuk menambah kerusakan
        [HttpPost("addKerusakan")]
        public IActionResult AddKerusakan([FromBody] Kerusakan kerusakan)
        {
            kerusakanList = AddItem(kerusakanList, kerusakan);  // Menambah kerusakan dengan generics
            return CreatedAtAction(nameof(GetKerusakan), new { platNomor = kerusakan.PlatNomor }, kerusakan);
        }

        // API untuk menghapus kerusakan berdasarkan platNomor
        [HttpDelete("deleteKerusakan/{platNomor}")]
        public IActionResult DeleteKerusakan(string platNomor)
        {
            var kerusakan = kerusakanList.Find(k => k.PlatNomor == platNomor);
            if (kerusakan != null)
            {
                kerusakanList.Remove(kerusakan);
                return NoContent();
            }
            return NotFound("Kerusakan tidak ditemukan!");
        }
 


        private static List<T> AddItem<T>(List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
     

    }
}
