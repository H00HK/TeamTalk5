namespace TeamTalkLib
{
    public interface IStorage<K>
    {
        void Store(K data);
    }
}