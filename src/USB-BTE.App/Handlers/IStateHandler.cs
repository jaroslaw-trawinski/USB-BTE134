namespace USB_BTE.App.Handlers
{
    public interface IStateHandler
    {
        bool GetState();
        void SetState(bool state);
    }
}
