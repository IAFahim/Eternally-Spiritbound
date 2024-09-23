namespace Soul2.ChainOfResponsibility.Runtime
{
    public interface IResponsibilityHandler<T, TV>
    {
        IResponsibilityHandler<T, TV> Next { get; set; }
        TV Handle(T controller);
        TV HandleNext(T controller);
    }

    public interface IResponsibilityHandler<T>
    {
        IResponsibilityHandler<T> Next { get; set; }
        void Handle(T responsibility);
        void HandleNext(T responsibility);
    }
}