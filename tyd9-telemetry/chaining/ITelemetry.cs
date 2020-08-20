using System.Threading.Tasks;

namespace Company.Function
{
    public interface ITelemetry
    {
        void Event(Event telemetryEvent);
        void Error(Error error);
        Task<Dependency> Dependency(Dependency dependency);
        void SetUserContext(string userId);
        void SetOperationContext(string executionId);
        void StartOperation(string operationName, string operationId, string parentOperationId = null);
        void StopOperation();
    }
}