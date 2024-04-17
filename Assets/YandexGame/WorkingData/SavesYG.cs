
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public float _soundAmount = 0.5f;
        public float _musicAmount = 0.1f;
        public int _playerLVL = 1;
        public int _playerSwordCount = 1;
        public int _playerSkin = 0;
        public int _playerCoins = 0;
        public int _playerRotationSpeed = 50;
        public int _bestKill = 0;
        public int[] _skins = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public SavesYG()
        {
            
        }
    }
}
