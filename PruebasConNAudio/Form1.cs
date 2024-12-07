using NAudio.CoreAudioApi;


namespace PruebasConNAudio
{
    public partial class Form1 : Form
    {
        private const string MUTEADO_MSG = "Dando publicidad, Muteado ;)";
        private const string APP2MUTE = "spotify";
        string lastSong = "";
        SimpleAudioVolume spotifyolume;


        public Form1()
        {
            InitializeComponent();
            Visible = false;
            iniDb();
        }



        private void iniDb()
        {
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            string currentSong = ShowMeSpotify();
            if (currentSong.Length > 0 && !currentSong.Equals(lastSong))
            {
                spotifyolume.Mute = false;
                labelTitle.Text = currentSong;
                lastSong = currentSong;
                notifyIcon1.Text = currentSong;
                notifyIcon1.BalloonTipText = currentSong;
                notifyIcon1.ShowBalloonTip(1);
                using (AppDbContext db = new AppDbContext())
                {
                    if (db.blackList.Any(n => n.windowTitle.Equals(currentSong)))
                    {
                        DandoPubli();
                    }
                }
            }
        }


        private void DandoPubli()
        {
            spotifyolume.Mute = true;
            notifyIcon1.BalloonTipText = MUTEADO_MSG;
            notifyIcon1.ShowBalloonTip(1);
        }

        private string ShowMeSpotify()
        {
            string titulo = "";
            // Crear un enumerador para los dispositivos de audio
            var enumerator = new MMDeviceEnumerator();
            enumerator = new MMDeviceEnumerator();

            // Obtener el dispositivo de audio de salida predeterminado
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            // Obtener el administrador de sesiones de audio del dispositivo
            AudioSessionManager sessionManager = device.AudioSessionManager;

            // Obtener todas las sesiones de audio activas
            SessionCollection sessions = sessionManager.Sessions;

            // Enumerar todas las sesiones
            for (int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];
                //var sessionControl = session.DisplayName..Control;

                // Obtener el nombre del programa (si está disponible)
                string sessionIdentifier = session.DisplayName;
                string[] processName = GetProcessName(session.GetProcessID);
                if (processName.Length == 1) processName.Append(" ");
                if (processName[0].ToLower().Contains(APP2MUTE))
                {
                    if (processName[1].Length > 0)
                    {
                        spotifyolume = session.SimpleAudioVolume;
                        titulo = processName[1];
                        return titulo;
                    }
                    session.SimpleAudioVolume.Volume = 0;
                }
            }
            return titulo;
        }

        private string[] GetProcessName(uint processId)
        {
            try
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById((int)processId);
                return [process.ProcessName, process.MainWindowTitle];
            }
            catch (Exception ex)
            {
                return [$"Proceso desconocido (Error: {ex.Message})"];
            }
        }


        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (!notifyIcon1.BalloonTipText.Equals(lastSong)) return;
            using (var db = new AppDbContext())
            {
                BlackItem item = new BlackItem() { windowTitle = lastSong };
                db.blackList.Add(item);
                db.SaveChanges();
            }
            DandoPubli();
        }



        private void MostrarForm2()
        {
            // mostrar el formulario form2
            Form2 form = new Form2();
            form.Show();

        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void viewBlackListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MostrarForm2();
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MostrarForm2();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }
    }
}
