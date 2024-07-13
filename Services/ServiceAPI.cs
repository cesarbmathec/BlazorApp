using System.Net.Http.Headers;
using System.Text;
using BlazorApp.Models;
using System.Text.Json;
using BlazorApp.Interfaces;

namespace BlazorApp.Services
{
    public class ServiceAPI : IServiceAPI
    {
        private readonly HttpClient _httpClient;
        private string _baseURL = "http://127.0.0.1:8000/";
        private string? _token;

        public ServiceAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseURL);
        }

        private async Task<bool> Authenticate()
        {
            if (_token != null) return true;

            var username = "edixoncesar";
            var password = "edixoncesar";

            var credentials = new Credential { username = username, password = password };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api-token-auth/", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Token>(jsonResponse);
                    if (result != null)
                    {
                        _token = result.token;
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _token);
                        if (_token != null)
                        {
                            Console.WriteLine("TOKEN: ", _token);
                            return true;
                        }
                    }
                }

            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            return false;
        }

        public async Task<bool> CreatePaciente(Paciente paciente)
        {
            bool auth = await Authenticate();

            var data = new
            {
                nombre = paciente.nombre,
                apellido = paciente.apellido,
                fecha_nacimiento = paciente.fecha_nacimiento,
                genero = paciente.genero,
                direccion = paciente.direccion,
                telefono = paciente.telefono,
                correo_electronico = paciente.correo_electronico,
                cedula_identidad = paciente.cedula_identidad
            };

            Console.WriteLine(JsonSerializer.Serialize(data));


            if (auth)
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PostAsync("/historias_medicas/paciente/", content);
                    return response.IsSuccessStatusCode;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }

            return false;
        }

        public async Task<bool> DeletePaciente(int id)
        {
            bool auth = await Authenticate();

            if (auth)
            {
                var response = await _httpClient.DeleteAsync($"historias_medicas/paciente/{id}/");

                return response.IsSuccessStatusCode;
            }

            return false;
        }

        public async Task<Paciente?> GetPaciente(int? id)
        {
            bool auth = await Authenticate();
            if (id != null)
            {
                if (auth)
                {
                    var response = await _httpClient.GetAsync($"historias_medicas/paciente/{id}/");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Paciente>(jsonResponse);
                    }
                }
            }

            return null;
        }

        public async Task<List<Paciente>?> GetPacienteList()
        {
            bool auth = await Authenticate();

            if (auth)
            {
                var response = await _httpClient.GetAsync("/historias_medicas/paciente/");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<Paciente>>(jsonResponse);
                    return result;
                }
            }

            return null;
        }

        public async Task<bool> UpdatePaciente(Paciente paciente)
        {
            await Authenticate();

            var content = new StringContent(JsonSerializer.Serialize(paciente), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"historias_medicas/paciente/{paciente.id}/", content);

            return response.IsSuccessStatusCode;
        }
    }
}
