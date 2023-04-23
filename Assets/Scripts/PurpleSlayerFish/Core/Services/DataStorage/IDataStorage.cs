namespace PurpleSlayerFish.Core.Services.DataStorage
{
    public interface IDataStorage<T> where T : struct
    {
        void Save(T data);
        T Load();
        void Clear();
    }
}