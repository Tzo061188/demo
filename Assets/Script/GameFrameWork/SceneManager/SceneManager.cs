using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonBase_MonoAuto<SceneLoadManager>
{
    public void LoadScene(string sceneName,UnityAction func) {
        SceneManager.LoadScene(sceneName);
        func?.Invoke();
    
    }
    public void AsyncLoadScene(string sceneName, UnityAction func) {
        StartCoroutine(AsyncLoad(sceneName,func));
        
    }

    private IEnumerator AsyncLoad(string sceneName, UnityAction func) {


        AsyncOperation operation =  SceneManager.LoadSceneAsync(sceneName);

        operation.completed += (AsyncOperationc)=>{
            
            if(operation.isDone == true){
                print("执行");
                func?.Invoke();
            }
        };

        GameManager.Instance.Show_LoadScene_UI_Animation(operation);

        yield return operation;


        

    }
}
