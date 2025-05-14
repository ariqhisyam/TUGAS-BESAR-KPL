using Microsoft.AspNetCore.Mvc;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Helpers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        
        private static List<Key> keyList = new List<Key>
        {
            new Key { Username = "admin", Role = "admin", KeyValue = "12345" },
             new Key { Username = "rey", Role = "driver", KeyValue = "1" }
        };
        private static List<T> AddItem<T>(List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
        // API untuk mendapatkan semua key
        [HttpGet("getKey")]
        public IActionResult GetKey()
        {
            return Ok(keyList);
        }

        // API untuk memperbarui key
        [HttpPut("updateKey/{username}")]
        public IActionResult UpdateKey(string username, [FromBody] Key updatedKey)
        {
            var key = keyList.Find(k => k.Username == username);
            if (key == null)
                return NotFound("Key tidak ditemukan!");

            key.Username = updatedKey.Username;
            key.Role = updatedKey.Role;
            key.KeyValue = updatedKey.KeyValue;
            return Ok("Key berhasil diperbarui!");
        }

        // API untuk menambah key
        [HttpPost("addKey")]
        public IActionResult AddKey([FromBody] Key key)
        {
            keyList = AddItem(keyList, key);  // Menambah key dengan generics
            return CreatedAtAction(nameof(GetKey), new { username = key.Username }, key);
        }

        // API untuk menghapus key berdasarkan username
        [HttpDelete("deleteKey/{username}")]
        public IActionResult DeleteKey(string username)
        {
            var key = keyList.Find(k => k.Username == username);
            if (key != null)
            {
                keyList.Remove(key);
                return NoContent();
            }
            return NotFound("Key tidak ditemukan!");
        }

        [HttpGet("getTanggalHariIni")]
        public IActionResult GetTanggalHariIni()
        {
            string tanggal = DateHelper.FormatIndo(DateTime.Now);
            return Ok($"Hari ini: {tanggal}");
        }
        [HttpPost("loginDriver")]
        public IActionResult loginDriver([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Key))
            {
                return BadRequest(new { success = false, message = "Username dan key harus diisi" });
            }

            var key = keyList.FirstOrDefault(k => k.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) && k.KeyValue == request.Key && k.Role == "driver");
            if (key != null)
            {

                return Ok(new { success = true, message = "Login berhasil", role = key.Role, key = key.KeyValue });
            }
            else
            {
                return Unauthorized(new { success = false, message = "Username atau key salah" });
            }
        }
        [HttpPost("loginAdmin")]
        public IActionResult LoginAdmin([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Key))
            {
                return BadRequest(new { success = false, message = "Username dan key harus diisi" });
            }

            var key = keyList.FirstOrDefault(k =>
                k.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) &&
                k.KeyValue == request.Key &&
                k.Role == "admin");

            if (key != null)
            {
                return Ok(new
                {
                    success = true,
                    message = "Login berhasil sebagai admin",
                    role = key.Role,
                    key = key.KeyValue
                });
            }
            else
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Username atau key salah"
                });
            }
        }
        [HttpPost("loginTeknisi")]
        public IActionResult LoginTeknisi([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Key))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Username dan key harus diisi"
                });
            }

            var key = keyList.FirstOrDefault(k =>
                k.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) &&
                k.KeyValue == request.Key &&
                k.Role == "teknisi");

            if (key != null)
            {
                return Ok(new
                {
                    success = true,
                    message = "Login berhasil sebagai teknisi",
                    role = key.Role,
                    key = key.KeyValue 
                });
            }

            return Unauthorized(new
            {
                success = false,
                message = "Username/key salah atau tidak memiliki akses teknisi"
            });
        }
      
    }
}
