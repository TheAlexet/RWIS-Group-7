using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // [SerializeField] Database db;
    // [SerializeField] TMPro.TextMeshProUGUI acornsText;
    [SerializeField] private AudioSource buttonSound;
    // public void ChangeScene(int sceneID)
    // {
    //     int level = sceneID - 1;
    //     int acorns = (level - 1) * 100;

    //     if(db.getMaxLevel() >= level)
    //     {
    //         SceneManager.LoadScene(sceneID);
    //     }
    //     else if(db.getAcorns() >= acorns && level < 3)
    //     {
    //         db.setMaxLevel(db.getMaxLevel() + 1);
    //         db.setAcorns(db.getAcorns() - acorns);
    //         acornsText.text = db.getAcorns().ToString();
    //     }
    // }

    public void ChangeScene(int sceneID) { 
        SceneManager.LoadScene(sceneID);
        buttonSound.Play();
    }
}
