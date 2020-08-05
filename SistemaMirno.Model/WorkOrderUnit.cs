namespace SistemaMirno.Model
{
    public class WorkOrderUnit : BaseModel
    {
        public int WorkOrderId { get; set; }

        public WorkOrder WorkOrder { get; set; }

        public int WorkUnitId { get; set; }

        public WorkUnit WorkUnit { get; set; }
    }
}
