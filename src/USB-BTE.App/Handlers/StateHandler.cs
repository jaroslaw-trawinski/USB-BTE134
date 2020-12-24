namespace USB_BTE.App.Handlers
{

    public class StateHandler : IStateHandler
    {
        public bool GetState()
        {
            return Properties.Settings.Default.RaleysAreOpen;
        }

        public void SetState(bool state)
        {
            Properties.Settings.Default.RaleysAreOpen = state;
            Properties.Settings.Default.Save();
        }
    }
}