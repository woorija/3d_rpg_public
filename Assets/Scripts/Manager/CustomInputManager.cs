public class CustomInputManager : SingletonBehaviour<CustomInputManager>
{
    public Controls customInputActionAsset {  get; private set; }

    public Controls.PlayerActions Player => customInputActionAsset.Player;
    public Controls.MyUIActions UI => customInputActionAsset.MyUI;
    public Controls.ManagerActions Manager => customInputActionAsset.Manager;

    protected override void Awake()
    {
        base.Awake();

        customInputActionAsset = new Controls();

        EnableAllActionMap();
    }
    void EnableAllActionMap()
    {
        Player.Enable();
        UI.Enable();
        Manager.Enable();
    }
    public void EnablePlayerActionMap()
    {
        Player.Enable();
    }
    public void DisablePlayerActionMap()
    {
        Player.Disable();
    }
}
