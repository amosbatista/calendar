using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;
using calendar_3;
using System.Net;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(SourceGeneratorLambdaJsonSerializer<CustomSerializer>))]


namespace calendar_3;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private IDateService _calendarService;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <remarks>
    /// The <see cref="ICalculatorService"/> implementation that we
    /// instantiated in <see cref="Startup"/> will be injected here.
    /// 
    /// As an alternative, a dependency could be injected into each 
    /// Lambda function handler via the [FromServices] attribute.
    /// </remarks>
    
    public Functions()
    {
    }
    public Functions(
        IDateService dateService
    )
    {
        _calendarService = dateService;
    }

    /// <summary>
    /// Retrieve the next holiday
    /// </summary>
    /// <returns>the next holiday, and how much time to come</returns>
    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, "/")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request)
    {
        CalendarResult result = _calendarService.GetTheNextDateInsideCalendar(NationalList.National);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonSerializer.Serialize(result, CustomSerializer.Default.CalendarResult)
        };
    }
}

[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
[JsonSerializable(typeof(CalendarResult))]
public partial class CustomSerializer : JsonSerializerContext
{
    
}