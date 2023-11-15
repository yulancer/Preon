namespace Preon.Solver.Integration.Options;

public class WebApiClientOptions
{
    public string BaseUrl { get; set; }
    
    public int TimeOutSeconds { get; set; }
    
    public string ApiKey { get; set; }
    
    public string SendPersonCommand { get; set; }
    
    public int[] WaitingBetweenRetryAttemptsInSeconds { get; set; }
}