using UnityEngine;
using UnityEngine.SceneManagement; // これが必須


public class SceneController : MonoBehaviour
{
    // タイトルからゲームへ (Title -> Game)
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    // ゲームからリザルトへ (Game -> Result)
    public void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }


    // リザルトからタイトルへ (Result -> Title)
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
