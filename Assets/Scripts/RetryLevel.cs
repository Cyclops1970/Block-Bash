using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryLevel : MonoBehaviour {

	//current level will already be set elsehwere, as level just played

	public void Retry()
    {
        SceneManager.LoadScene("PlayGame");
    }
}
