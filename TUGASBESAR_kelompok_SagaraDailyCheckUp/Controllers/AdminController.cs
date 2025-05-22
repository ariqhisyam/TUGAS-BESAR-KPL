using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Helpers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private static List<Kendaraan> kendaraanList = new List<Kendaraan>
        {
            new Kendaraan { Merek = "Toyota", PlatNomor = "AB 123 CD" },
            new Kendaraan { Merek = "Honda", PlatNomor = "EF 456 GH"},
            new Kendaraan { Merek = "BMW", PlatNomor = "XY 789 ZT"},
        };

        private static List<T> AddItem<T>(List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        private static string configFilePath = "path/to/config.txt";

        [HttpGet("getKendaraan")]
        public IActionResult GetKendaraan()
        {
            return Ok(kendaraanList);
        }

        [HttpGet("getKendaraan/{platNomor}")]
        public IActionResult GetKendaraan(string platNomor)
        {
            var kendaraan = kendaraanList.FirstOrDefault(k => k.PlatNomor == platNomor);
            if (kendaraan != null)
                return Ok(kendaraan);
            return NotFound("Kendaraan tidak ditemukan!");
        }

        // ✅ Table-Driven untuk update kendaraan
        [HttpPut("updateKendaraan/{platNomor}")]
        public IActionResult UpdateKendaraan(string platNomor, [FromBody] Kendaraan updatedKendaraan)
        {
            var kendaraan = kendaraanList.FirstOrDefault(k => k.PlatNomor == platNomor);
            if (kendaraan == null)
                return NotFound("Kendaraan tidak ditemukan!");

            // Table-driven update mapping
            var updateActions = new Dictionary<string, Action>
            {
                { "Merek", () => kendaraan.Merek = updatedKendaraan.Merek },
                { "PlatNomor", () => kendaraan.PlatNomor = updatedKendaraan.PlatNomor }
            };

            // Jalankan semua aksi update
            foreach (var action in updateActions.Values)
            {
                action();
            }

            return Ok("Kendaraan berhasil diperbarui!");
        }

        [HttpPost("addKendaraan")]
        public IActionResult AddKendaraan([FromBody] Kendaraan kendaraan)
        {
            kendaraanList = AddItem(kendaraanList, kendaraan);
            return CreatedAtAction(nameof(GetKendaraan), new { platNomor = kendaraan.PlatNomor }, kendaraan);
        }

        [HttpDelete("deleteKendaraan/{platNomor}")]
        public IActionResult DeleteKendaraan(string platNomor)
        {
            var kendaraan = kendaraanList.Find(k => k.PlatNomor == platNomor);
            if (kendaraan != null)
            {
                kendaraanList.Remove(kendaraan);
                return NoContent();
            }
            return NotFound("Kendaraan tidak ditemukan!");
        }
    }
}
