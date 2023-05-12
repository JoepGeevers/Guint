using Guint;

public class SysTrayApp : Form
{
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;
    private System.Windows.Forms.Timer clipboardCheckTimer;
    private string previous;

    (string key, string vector) Pair = ("", "");

    [STAThread]
    public static void Main()
    {
        Application.Run(new SysTrayApp());
    }

    public SysTrayApp()
    {
        trayMenu = new ContextMenuStrip();
        trayMenu.Items.Add("Exit", null, OnExit);

        trayIcon = new NotifyIcon();
        trayIcon.Text = "Clipboard Monitor";
        trayIcon.Icon = new Icon("guint.nuget.icon.ico");

        trayIcon.ContextMenuStrip = trayMenu;
        trayIcon.Visible = true;

        clipboardCheckTimer = new System.Windows.Forms.Timer();
        clipboardCheckTimer.Interval = 1000; // Check every second
        clipboardCheckTimer.Tick += CheckClipboard;
        clipboardCheckTimer.Start();
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false; // Hide form window
        ShowInTaskbar = false; // Remove from taskbar

        base.OnLoad(e);
    }

    private void OnExit(object sender, EventArgs e)
    {
        Application.Exit();
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            // Release the icon resource
            trayIcon.Dispose();
        }

        base.Dispose(isDisposing);
    }

    private void CheckClipboard(object sender, EventArgs e)
    {
        if (Clipboard.ContainsText())
        {
            string current = Clipboard.GetText();

            if (current == previous)
            {
                return;
            }

            previous = current;

            if (int.TryParse(current, out int i))
            {
                var encrypted = i.EncryptIntoGuid(this.Pair.key, this.Pair.vector);

                previous = encrypted.ToString();
                Clipboard.SetText(encrypted.ToString());

                ShowBalloon($"An integer `{i}` was found on the clipboard. I copied the corresponding Guid `{encrypted}` onto the clipboard");
            }
            else if (Guid.TryParse(current, out var g))
            {
                var decrypted = g.DecryptToInt(this.Pair.key, this.Pair.vector);

                if (decrypted.HasValue)
                {
                    previous = decrypted.Value.ToString();
                    Clipboard.SetText(decrypted.Value.ToString());

                    ShowBalloon($"A guid `{g}` was found on the clipboard. I copied the corresponding int `{decrypted}` onto the clipboard");
                }
            }
        }
    }

    private void ShowBalloon(string message)
    {
        trayIcon.BalloonTipText = message;
        trayIcon.ShowBalloonTip(1000);
    }
}