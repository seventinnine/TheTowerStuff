using System.Net.Http.Json;

namespace Website.Services
{
    public class InternalDataLookup(HttpClient httpClient)
    {
        public async Task<Dictionary<T1, T2>?> LoadAsync<T1, T2>(string filename)
            where T1 : notnull
        {
            return await httpClient.GetFromJsonAsync<Dictionary<T1, T2>>(filename);
        }
    }
}
