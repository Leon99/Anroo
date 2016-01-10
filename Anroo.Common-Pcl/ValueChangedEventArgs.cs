namespace Anroo.Common
{
    public struct ValueChangedEventArgs<T>
    {
        public T OldValue;
        public T NewValue;

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}