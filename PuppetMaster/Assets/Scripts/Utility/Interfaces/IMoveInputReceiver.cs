namespace Player.Input
{
    public interface IMoveInputReceiver
    {
        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }
    }
}