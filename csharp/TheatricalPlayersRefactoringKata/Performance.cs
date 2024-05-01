namespace TheatricalPlayersRefactoringKata
{
    public class Performance
    {
        private string _playName;
        private int _audience;

        public string PlayName { get => _playName; set => _playName = value; }
        public int Audience { get => _audience; set => _audience = value; }

        public Performance(string playName, int audience)
        {
            this._playName = playName;
            this._audience = audience;
        }

    }
}
