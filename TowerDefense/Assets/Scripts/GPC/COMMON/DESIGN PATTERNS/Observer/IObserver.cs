namespace Harris.GPC
{
    public interface IObserver
    {
        // Receive update from subject
        void Update(ISubject subject);
    }
}