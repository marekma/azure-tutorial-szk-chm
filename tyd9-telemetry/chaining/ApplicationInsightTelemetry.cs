using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    public class ApplicationInsightTelemetry : ITelemetry
    {
        public TelemetryClient ApplicationInsightsClient { get; private set; }

        public async Task<Dependency> Dependency(Dependency dependency)
        {
            this.ApplicationInsightsClient.TrackDependency(
                new DependencyTelemetry()
                {
                    Name = dependency.Name,
                    Data = await dependency.Response.Content.ReadAsStringAsync(),
                    Duration = dependency.Duration,
                    Success = dependency.Response.IsSuccessStatusCode,
                    ResultCode = dependency.Response.StatusCode.ToString(),
                    Timestamp = DateTime.UtcNow
                });
            return dependency;
        }

        public void Error(Error error)
        {
            throw new System.NotImplementedException();
        }

        public static ITelemetry Create(TelemetryClient client)
        {
            return new ApplicationInsightTelemetry
            {
                ApplicationInsightsClient = client
            };
        }

        public void SetUserContext(string userId)
        {
            ApplicationInsightsClient.Context.User.Id = userId;
        }

        public void SetOperationContext(string executionId)
        {
            ApplicationInsightsClient.Context.Operation.Id = executionId;
        }

        private IOperationHolder<DependencyTelemetry> operation = null;

        public void StartOperation(string operationName, string operationId, string parentOperationId = null)
        {
            operation = ApplicationInsightsClient.StartOperation<DependencyTelemetry>(operationName, operationId, parentOperationId);
        }

        public void StopOperation()
        {
            ApplicationInsightsClient.StopOperation(operation);
        }

        public void Event(Event tEvent)
        {
            this.ApplicationInsightsClient.TrackEvent(
                new EventTelemetry()
                {
                    Timestamp = DateTime.UtcNow,
                    Name = tEvent.Name
                }
            );
        }

    }
}