using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button killButton;

    private DaemonGame game;

    private Coroutine tut;

    public GameObject stage0, stage1, stage2, stage3, stage4;

    // Start is called before the first frame update
    void Start()
    {
        game = DaemonGame.GetInstance();

        killButton.onClick.AddListener(KillTut);

        Restart();
    }

    private void Restart()
    {
        if (tut != null)
        {
            StopCoroutine(tut);
            tut = null;
        }
        tut = StartCoroutine(DoTutorial());
    }

    private IEnumerator DoTutorial()
    {
        game.myLearning = new DaemonGame.LearningStages();
        stage0.gameObject.SetActive(false);
        stage1.gameObject.SetActive(false);
        stage2.gameObject.SetActive(false);
        stage3.gameObject.SetActive(false);
        stage4.gameObject.SetActive(false);

        while (FindObjectOfType<Daemon>() == null)
        {
            yield return new WaitForFixedUpdate();
        }

        // Stage 0
        stage0.gameObject.SetActive(true);
        while (!game.myLearning.HasClickedDaemon)
        {
            yield return new WaitForEndOfFrame();
        }
        stage0.gameObject.SetActive(false);

        // Stage 1
        stage1.gameObject.SetActive(true);
        while (!game.myLearning.HasRemovedAction)
        {
            yield return new WaitForEndOfFrame();
        }
        stage1.gameObject.SetActive(false);

        // Stage 2
        stage2.gameObject.SetActive(true);
        while (!game.myLearning.HasClickedActionFromStore)
        {
            yield return new WaitForEndOfFrame();
        }
        stage2.gameObject.SetActive(false);

        // Stage 3
        stage3.gameObject.SetActive(true);
        while (!game.myLearning.HasAddedAction)
        {
            yield return new WaitForEndOfFrame();
        }
        stage3.gameObject.SetActive(false);

        // Delay for Stage 4
        while (!game.myLearning.HasGibletAppeared)
        {
            yield return new WaitForEndOfFrame();
        }

        // Stage 4
        stage4.gameObject.SetActive(true);
        while (!game.myLearning.HasCollectedGiblet)
        {
            yield return new WaitForEndOfFrame();
        }
        stage4.gameObject.SetActive(false);

        KillTut();
    }

    private void KillTut()
    {
        gameObject.SetActive(false);
    }
}
