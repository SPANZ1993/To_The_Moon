using System.Collections;

public class LimitedQueue<T> : Queue
{
    public int Limit { get; set; }

    public LimitedQueue(int limit) : base(limit)
    {
        Limit = limit;
    }

    public new void Enqueue(T item)
    {
        while (Count >= Limit)
        {
            Dequeue();
        }
        base.Enqueue(item);
    }
}
