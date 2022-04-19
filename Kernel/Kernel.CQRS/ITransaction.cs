namespace Kernel.Messaging.Transaction
{
    public interface ITransaction
    {
        void Begin();
        void Commit();
        void Abort();
    }
}