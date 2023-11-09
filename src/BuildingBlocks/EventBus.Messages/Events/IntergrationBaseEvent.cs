namespace EventBus.Messages.Events
{
    public class IntergrationBaseEvent
    {
        public IntergrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
        }

        public IntergrationBaseEvent(Guid id, DateTime creationTime)
        {
            Id = id;
            CreationTime = creationTime;
        }

        public Guid Id { get; private set; }
        public DateTime CreationTime { get; private set; }
    }
}
