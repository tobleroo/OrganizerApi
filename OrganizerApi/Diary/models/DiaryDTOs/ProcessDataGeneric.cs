namespace OrganizerApi.Diary.models.DiaryDTOs
{
    public class ProcessDataGeneric<T> : ProcessData
    {
        public T Data { get; set; } = default(T);
    }
}
