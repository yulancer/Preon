using System.Net;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Polly;
using Preon.Solver.Contracts.Abstractions;
using Preon.Solver.Contracts.Models;
using Preon.Solver.Integration.Contracts;
using Preon.Solver.Integration.Options;

namespace Preon.Solver.Integration.Services;

public class WebApiClient : IWebApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WebApiClientOptions _options;
    private readonly ILogger<WebApiClient> _logger;
    private readonly IMapper _mapper;
        
    /// <summary> Ответы, при которых повторять запросы смысла не имеет </summary>
    private readonly List<HttpStatusCode> httpStatusCodesWorthRetrying = new List<HttpStatusCode> {
        HttpStatusCode.BadRequest, // 400
        HttpStatusCode.NotFound, // 404
    };

    public WebApiClient(
        HttpClient httpClient,
        WebApiClientOptions options,
        ILogger<WebApiClient> logger,
        IMapper mapper)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task SendPerson(PersonModel personModel, CancellationToken token)
    {
        var url = CreateUrl();
        var contract = _mapper.Map<PersonContract>(personModel);
        string body = JsonSerializer.Serialize(contract);

        var responsePolicy = await GetPolicy(() => _httpClient.PostAsync(url, CreateJsonContent(body), token));
        var content = await responsePolicy.Result.Content.ReadAsStringAsync(token);
        if (responsePolicy.Outcome is OutcomeType.Failure || httpStatusCodesWorthRetrying.Contains(responsePolicy.Result.StatusCode))
        {
            string message = $"Произошла ошибка при запросе к сервису запретов. " +
                             $"ErrorMessage: {content} FinalException: {responsePolicy.FinalException}";
            _logger.LogError(message);
            throw new HttpRequestException(message);
        }
    }

    private Uri CreateUrl()
    {
        var uriBuilder = new UriBuilder
        {
            Path = _options.SendPersonCommand,
            Query = $"ApiKey={_options.ApiKey}"
        };
        return uriBuilder.Uri;
    }
    private async Task<PolicyResult<HttpResponseMessage>> GetPolicy(Func<Task<HttpResponseMessage>> action) => 
        await Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(ContainsRetryableErrorCodes)               
            .WaitAndRetryAsync(GetRequestDelayTime(), OnRetry)
            .ExecuteAndCaptureAsync(action);
        
    private bool ContainsRetryableErrorCodes(HttpResponseMessage message) =>
        !message.IsSuccessStatusCode &&
        !httpStatusCodesWorthRetrying.Contains(message.StatusCode);
        
    private TimeSpan[] GetRequestDelayTime() =>
        _options.WaitingBetweenRetryAttemptsInSeconds.Select(t => TimeSpan.FromSeconds(t)).ToArray();
        
    private Task OnRetry(DelegateResult<HttpResponseMessage> exception, TimeSpan time, Context context)
    {
        _logger.LogError("Не удалось выполнить запрос к сервису. Time: {Seconds}s: {@Exception}", time.TotalSeconds, exception);
        return Task.CompletedTask;
    }
    private HttpContent CreateJsonContent(string content) =>
        new StringContent(content, System.Text.Encoding.UTF8, "application/json");
}