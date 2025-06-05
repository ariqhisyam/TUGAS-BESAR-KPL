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
            new Kendaraan { Merek = "Toyota", PlatNomor = "AB123CD" },
            new Kendaraan { Merek = "Honda", PlatNomor = "EF456GH"},
            new Kendaraan { Merek = "BMW", PlatNomor = "XY789ZT"},

        };




        // Menggunakan generics dan parameterization untuk menambah item
        private static List<T> AddItem<T>(List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        // Runtime configuration
        private static string configFilePath = "path/to/config.txt"; 

        // API untuk mendapatkan semua kendaraan
        [HttpGet("getKendaraan")]
        public IActionResult GetKendaraan()
        {
            var orderedKendaraanList = kendaraanList.AsEnumerable().Reverse().ToList();
            return Ok(orderedKendaraanList);
        }

        // API untuk mendapatkan kendaraan berdasarkan platNomor
        [HttpGet("getKendaraan/{platNomor}")]
        public IActionResult GetKendaraan(string platNomor)
        {
            var kendaraan = kendaraanList.FirstOrDefault(k => k.PlatNomor == platNomor);
            if (kendaraan != null)
                return Ok(kendaraan);
            return NotFound("Kendaraan tidak ditemukan!");
        }

        // API untuk memperbarui kendaraan (menggunakan Automata dan Table-driven construction)
        [HttpPut("updateKendaraan/{platNomor}")]
        public IActionResult UpdateKendaraan(string platNomor, [FromBody] Kendaraan updatedKendaraan)
        {
            var kendaraan = kendaraanList.Find(k => k.PlatNomor == platNomor);
            if (kendaraan == null)
                return NotFound("Kendaraan tidak ditemukan!");

            kendaraan.Merek = updatedKendaraan.Merek;
            kendaraan.PlatNomor = updatedKendaraan.PlatNomor;
            return Ok("Kendaraan berhasil diperbarui!");
        }

        // API untuk menambah kendaraan
        [HttpPost("addKendaraan")]
        public IActionResult AddKendaraan([FromBody] Kendaraan kendaraan)
        {
            kendaraanList = AddItem(kendaraanList, kendaraan);  
            return CreatedAtAction(nameof(GetKendaraan), new { platNomor = kendaraan.PlatNomor }, kendaraan);
        }

        // API untuk menghapus kendaraan berdasarkan platNomor
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
