namespace Erilipah.Runnables
{
    public abstract class Runnable
    {
        public virtual bool Active { get; protected set; } = true;

        public void End() => Active = false;

        public virtual void OnEnd() { }

        public abstract void Run();
    }
}
