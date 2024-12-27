namespace App.Domain.Events
{
    // nesne oluştuktan sonra değişiklik yapılmasın diye sadece record tanımladık.
    public record ProductAddedEvent(int Id, string Name, decimal Price) : IEventOrMessage;

    // public int ProductAddedEvent { get; init; } init ile yapıyorsun .yukarıdaki işlemi

}
