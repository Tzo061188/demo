using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class StartPanel : MonoBehaviour
{
    public Button buttonStart;
    public Button buttonContinue;
    public Button buttonExit;

		
    public void NewGame()
    {   
        SceneLoadManager.Instance.AsyncLoadScene("Game01",()=>{

            LuaManager.Instance.Init();
            LuaManager.Instance.DoLuaFile("Main");

            GameManager.Instance.mainPanel = GameManager.Instance.Canvas.Find("MainPanel(Clone)").GetComponent<MainPanel>();
            

            Cursor.lockState = CursorLockMode.Locked;
            
            GameManager.Instance.CreatePlayer();
            this.gameObject.SetActive(false);
        });
    }
    public void ContinueGame()
    {
        NewGame();
	}
  	public void ExitGame(){

        Application.Quit();
    }


}