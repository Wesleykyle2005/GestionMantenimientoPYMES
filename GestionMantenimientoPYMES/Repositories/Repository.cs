﻿using GestionMantenimientoPYMES.Repositories.IRepositories;
using Newtonsoft.Json;
using System.Text;

namespace GestionMantenimientoPYMES.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public Repository(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<T> CreateAsync(object dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseData);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_endpoint);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Status: {response.StatusCode}\n{errorResponse}");
        }
    }


    public async Task<T> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<bool> UpdateAsync(int id, object dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_endpoint}/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }
}