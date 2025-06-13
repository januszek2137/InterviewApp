using InterviewApp.Models;

namespace InterviewApp.Services {
    public class PhonesService {
        private readonly HttpClient _client;

        public PhonesService(HttpClient client) {
            _client = client;
        }

        public async Task<List<PhoneModel>> GetPhonesAsync() {
            var response = await _client.GetAsync("/api/Phones");
            if(response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<List<PhoneModel>>() ?? new List<PhoneModel>();
            }
            return new List<PhoneModel>();
        }

        public async Task<PhoneModel?> GetPhoneAsync(Guid id) {
            var response = await _client.GetAsync($"/api/Phones/{id}");
            if(response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<PhoneModel>();
            }
            return null;
        }

        public async Task<bool> CreatePhoneAsync(PhoneModel phone) {
            var response = await _client.PostAsJsonAsync("/api/Phones", phone);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePhoneAsync(Guid id, PhoneModel phone) {
            var response = await _client.PutAsJsonAsync($"/api/Phones/{id}", phone);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePhoneAsync(Guid id) {
            var response = await _client.DeleteAsync($"/api/Phones/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
