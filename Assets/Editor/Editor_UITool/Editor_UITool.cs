using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Editor_UITool : EditorWindow
{
   
    //���˵�û�����Ŀؼ� �� ȥ���ظ��� ������
    private List<string> filter = new List<string>() { "Image", 
                                                        "Text (Legacy)", 
                                                        "Button (Legacy)", 
                                                        "Text", 
                                                        "Background",
                                                        "",
                                                        "Checkmark",
                                                        "Fill",
                                                        "Handle",
                                                        "Label",
                                                        "Viewport",



    };
    private static Dictionary<string,Type>  unRepeat = new Dictionary<string,Type>();
    //gui��ʾ��λ����Ϣ
    Vector2 scrollpos1;
    Vector2 scrollpos2;
    //���յĽű��ַ���
    string uiBaseScript;
    string uiScript;

    //ѡ�еĶ���
    GameObject uiPanel;

    [MenuItem("Custom_Editor/UI/�Զ��������ű��ļ�")]
    public static void ExpandUIFunc() {
        Editor_UITool win =  EditorWindow.GetWindow<Editor_UITool>();
        win.titleContent = new GUIContent("�Զ��������ű��ļ�");
    }

    private void OnGUI()
    {

        scrollpos1 = EditorGUILayout.BeginScrollView(scrollpos1);
        GUILayout.Label(uiBaseScript);
        GUILayout.EndScrollView();

        EditorGUILayout.LabelField("-----------------------------------------");

        scrollpos2 = EditorGUILayout.BeginScrollView(scrollpos2);
        GUILayout.Label(uiScript);
        GUILayout.EndScrollView();


        if (GUILayout.Button("���ɽű�")) 
           if(Selection.activeGameObject != null)
                InitInfo();       
        
        if (GUILayout.Button("ѡ�񱣴��ļ���λ��")) {
            string path = EditorUtility.SaveFilePanel("�����ļ�", Application.dataPath+"/Scripts", uiPanel.name + "Base", "cs");
            if (path != "" && uiBaseScript != "") {
                File.WriteAllText(path, uiBaseScript);
                //�ַ�����ȡ�޸��ļ���
                int index =  path.LastIndexOf("Base");
                string uiPanelPath =  path.Substring(0, index) + ".cs";
                //��������� �����ļ� ��ֹ�Ѿ�д������ ��Ҫȥ�޸���
                if (!File.Exists(uiPanelPath)) {
                    File.WriteAllText(uiPanelPath, uiScript);
                }
                AssetDatabase.Refresh();
            }
        }
    }

    //�߼�
    public void InitInfo()
    {
        //ѡ�����巢���ı�ʱ ��� ȥ���ֵ�
        unRepeat.Clear();
        uiBaseScript = string.Empty;
        uiScript = string.Empty;

        //ѡ��ui���
        uiPanel =  Selection.activeGameObject;

        ControlStrInfo strinfo = new ControlStrInfo();
        strinfo += GetControlStrInfo<Button>(uiPanel);
        strinfo += GetControlStrInfo<Toggle>(uiPanel);
        strinfo += GetControlStrInfo<Slider>(uiPanel);
        strinfo += GetControlStrInfo<Image>(uiPanel);
        strinfo += GetControlStrInfo<Text>(uiPanel);
        //ƴ��ģ���ļ� 
        //����
        TextAsset uiBasetxt =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets\\Editor\\Editor_UITool\\TextConfig\\UIPanelBase.txt");
        uiBaseScript = string.Format(uiBasetxt.text, uiPanel.name, strinfo.controlName, strinfo.findStr, strinfo.addEventStr, strinfo.funcStr);
        //�̳���
        TextAsset uitxt = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets\\Editor\\Editor_UITool\\TextConfig\\UIPanel.txt");
        uiScript = string.Format(uitxt.text, uiPanel.name,uiPanel.name);
  
    }

    /// <summary>
    /// �ҵ�ui����µ�T���Ϳؼ� �����ַ���ƴ��
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="uiPanel">ѡ�е�UI���</param>
    /// <returns></returns>
    protected ControlStrInfo GetControlStrInfo<T>(GameObject uiPanel) where T : UIBehaviour
    {
        //���ҿؼ�
        T[] controls = uiPanel.GetComponentsInChildren<T>();

        //������Ϣ�Ĵ洢
        ControlStrInfo strInfo = new ControlStrInfo();

        foreach (T control in controls)
        {
           
            //����
            if (filter.Contains(control.name) || control.name == uiPanel.name) {
                continue;
            }
            //ȥ��
            if (unRepeat.ContainsKey(control.name)) {

                if (unRepeat[control.name] == typeof(T))
                {
                    //��ʾ
                    EditorUtility.DisplayDialog("����", $"�ҵ����ظ����Ƶ�----{control.name}", "ȷ��");
                    return null;
                }
                else {
                    //ͬ�����ǲ�ͬ���� Ҳ���ܴ��� 
                    continue;
                }
            }
            unRepeat.Add(control.name, typeof(T));


            //��ȡstring��Ϣ
            strInfo.controlName += $"protected {typeof(T).Name} {control.name};\n\t";
            strInfo.findStr += $"{control.name} = this.transform.Find(\"{FindParentPath(control.transform,uiPanel.transform)}\").GetComponent<{typeof(T)}>();\n\t\t";
            switch (typeof(T).Name)
            {
                case "Button":
                    strInfo.addEventStr += $"{control.name}.onClick.AddListener(On{control.name}Click);\n\t\t";
                    strInfo.funcStr += $"protected virtual void On{control.name}Click(){{}}\n\t";
                    break;
                case "Toggle":
                    strInfo.addEventStr += $"{control.name}.onValueChanged.AddListener(On{control.name}ValueChanged);\n\t\t";
                    strInfo.funcStr += $"protected virtual void On{control.name}ValueChanged(bool isChange){{}}\n\t";
                    break;
                case "Slider":
                    strInfo.addEventStr += $"{control.name}.onValueChanged.AddListener(On{control.name}ValueChanged);\n\t\t";
                    strInfo.funcStr += $"protected virtual void On{control.name}ValueChanged(float value){{}}\n\t";
                    break;
                default:
                    break;
            }
            
        }
        return strInfo;
    }

    //�ҵ��ؼ��ĸ�����·��
    private string FindParentPath(Transform obj, Transform selected) {
        string path = obj.name;
        while (obj.parent != selected) {
            
            path = obj.parent.name + "/"+ path;
            obj = obj.parent;
        }
        return path;
    }
    
}
