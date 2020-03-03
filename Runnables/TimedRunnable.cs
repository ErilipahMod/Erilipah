using Erilipah.Runnables;

namespace Erilipah
{
    public abstract class TimedRunnable : Runnable
    {
        protected TimedRunnable(int timeLeft)
        {
            TimeLeft = timeLeft;
        }

        public int TimeLeft { get; protected set; }

        public override bool Active { get => TimeLeft > 0; protected set => TimeLeft = value ? TimeLeft : 0; }

        public override void Run()
        {
            TimeLeft--;
        }
    }
}
