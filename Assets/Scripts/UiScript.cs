using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiScripy : MonoBehaviour
{
    [SerializeField] private CharControl player;
    [SerializeField] private float endDelay = 5f;
    private float timer;
    [SerializeField] private EnemyCounter counter;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text enemiesText;
    [SerializeField] private TMP_Text victory;

    void Update() {
        
        if (player.uiLife > 0)
        {

            lifeText.text = "Vida: " + player.uiLife.ToString();

        }
        else{
            lifeText.text = "Muerto";
        }

        //contador de enemigos
        if (counter.enemiesAlive > 0)
        {
            enemiesText.text = "Enemigos: " + counter.enemiesAlive.ToString();
        }
        else
        {
            lifeText.gameObject.SetActive(false);
            enemiesText.gameObject.SetActive(false);
            victory.text = "¡Victoria!";
            timer += Time.deltaTime;
            if (timer >= endDelay)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
           

        }
        
    }
}
    