namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public interface IFocus
    {
        public bool IsMain { get; }
        public bool IsFocused { get; set; }
    }
}