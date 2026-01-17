namespace Score
{
    public static class ScoreManager
    {
        private static int _score = 0;
        public static int Score => _score;

        public static void AddScore(int amount)
        {
            _score += amount;
            ScoreDisplayer.Instance?.UpdateScoreText(_score);
        }

        public static void ResetScore()
        {
            _score = 0;
            ScoreDisplayer.Instance?.UpdateScoreText(_score);
        }
    }

}