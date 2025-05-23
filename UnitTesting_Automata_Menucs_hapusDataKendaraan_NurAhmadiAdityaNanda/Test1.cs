using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnitTesting_Automata_Menucs_hapusDataKendaraan_NurAhmadiAdityaNanda
{
    public class Kendaraan
    {
        public string Merek { get; set; }
        public string PlatNomor { get; set; }
    }

    [TestClass]
    public sealed class Test1
    {
        private static readonly string apiBaseUrl = "http://dummyapi.com"; // Dummy base URL
        private static readonly HttpClient client = new HttpClient(new MockHttpMessageHandler());
        private static bool isTestMode = false; // Tambahkan flag untuk unit test

        [TestMethod]
        public async Task TestMethod1()
        {
            isTestMode = true; // Aktifkan mode test

            var input = "B 1234 XYZ\nY\n";
            Console.SetIn(new StringReader(input));
            var output = new StringWriter();
            Console.SetOut(output);

            await DeleteKendaraan();

            string result = output.ToString();
            Assert.IsTrue(result.Contains("Kendaraan berhasil dihapus."), "Kendaraan seharusnya berhasil dihapus.");

            output.GetStringBuilder().Clear();

            await TampilkanDataKendaraan();

            result = output.ToString();
            Assert.IsTrue(result.Contains("Merek: Toyota"), "Data kendaraan seharusnya tampil.");
        }

        private static async Task DeleteKendaraan()
        {
            Console.Write("Masukkan Plat Nomor Kendaraan yang ingin dihapus (format: B 1234 XYZ): ");
            string inputPlat = Console.ReadLine().ToUpper();

            Regex regexFormat = new Regex(@"^([A-Z]{1,2})(\d{1,4})([A-Z]{1,3})$");
            if (regexFormat.IsMatch(inputPlat.Replace(" ", "")))
            {
                var match = regexFormat.Match(inputPlat.Replace(" ", ""));
                inputPlat = $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
            }

            string patternValid = @"^[A-Z]{1,2} [0-9]{1,4} [A-Z]{1,3}$";
            if (!Regex.IsMatch(inputPlat, patternValid))
            {
                Console.WriteLine("Format plat nomor tidak valid! Contoh: B 1234 XYZ");
                return;
            }

            Console.Write($"Apakah Anda yakin ingin menghapus kendaraan dengan plat {inputPlat}? (Y/N): ");
            string confirm = Console.ReadLine().Trim().ToUpper();
            if (confirm != "Y")
            {
                Console.WriteLine("Aksi dibatalkan.");
                return;
            }

            try
            {
                if (!isTestMode)
                    Console.Clear();

                var encodedPlat = Uri.EscapeDataString(inputPlat);
                var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKendaraan/{encodedPlat}");
                Console.WriteLine(response.IsSuccessStatusCode
                    ? "Kendaraan berhasil dihapus."
                    : $"Gagal menghapus kendaraan. Status: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
            }
        }

        private static async Task TampilkanDataKendaraan()
        {
            try
            {
                var response = await client.GetAsync($"{apiBaseUrl}/getKendaraan");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var kendaraanList = JsonSerializer.Deserialize<List<Kendaraan>>(jsonResponse);

                    if (kendaraanList != null && kendaraanList.Count > 0)
                    {
                        if (!isTestMode)
                            Console.Clear();

                        Console.WriteLine("Data Kendaraan:");
                        foreach (var kendaraan in kendaraanList)
                        {
                            Console.WriteLine($"Merek: {kendaraan.Merek}, Plat Nomor: {kendaraan.PlatNomor}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tidak ada data kendaraan.");
                    }
                }
                else
                {
                    Console.WriteLine($"Gagal mengambil data kendaraan. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan: " + ex.Message);
            }
        }

        // Simulasi HttpClient untuk unit test
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                if (request.Method == HttpMethod.Delete)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                }
                else if (request.Method == HttpMethod.Get)
                {
                    var kendaraanList = new List<Kendaraan>
                    {
                        new Kendaraan { Merek = "Toyota", PlatNomor = "B 1234 XYZ" }
                    };
                    string json = JsonSerializer.Serialize(kendaraanList);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = content });
                }

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }
    }
}
