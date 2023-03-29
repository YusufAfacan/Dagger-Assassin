using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int enemyAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyAmount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        Enemy.OnDeath += Enemy_OnDeath;


    }

    private void Enemy_OnDeath(object sender, System.EventArgs e)
    {
        enemyAmount--;

        if (enemyAmount <= 0)
        {
            Enemy.OnDeath -= Enemy_OnDeath;
            Invoke(nameof(RestartScene), 2f);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
