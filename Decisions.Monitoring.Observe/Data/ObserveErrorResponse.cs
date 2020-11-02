namespace Decisions.Monitoring.Observe.Data
{
    internal class ObserveErrorResponse
    {
        public int? emptyLogLines;
        public int? malformedLines;
        public int? oversizedLines;
        public int? successfulLines;
    }
}