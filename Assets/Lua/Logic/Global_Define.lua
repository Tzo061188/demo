---@diagnostic disable: undefined-global
---------全局定义------------

-- Unity Class

GameObject = CS.UnityEngine.GameObject;
Transform = CS.UnityEngine.Transform;
RectTransform = CS.UnityEngine.RectTransform;
RectTransformUtility =  CS.UnityEngine.RectTransformUtility;
Vector3 = CS.UnityEngine.Vector3;
Vector2 = CS.UnityEngine.Vector2;
Quaternion = CS.UnityEngine.Quaternion

TextAsset =  CS.UnityEngine.TextAsset;
SpriteAtlas =  CS.UnityEngine.U2D.SpriteAtlas;

Color = CS.UnityEngine.Color;

Input = CS.UnityEngine.Input;

MainCamera = CS.UnityEngine.Camera.main;
Cursor = CS.UnityEngine.Cursor;

List=  CS.System.Collections.Generic.List;

EventSystems = CS.UnityEngine.EventSystems
Application = CS.UnityEngine.Application
Time = CS.UnityEngine.Time;
-- UI
UI =  CS.UnityEngine.UI;

Button = UI.Button;
Text = UI.Text;
Image = UI.Image;
Toggle = UI.Toggle;
Slider = UI.Slider;
ScrollRect = UI.ScrollRect;

Canvas = GameObject.Find("Canvas").transform;
CanvasRect = GameObject.Find("Canvas"):GetComponent(typeof(RectTransform))


-- MyCustomClass
RoleInputSystem = CS.RoleInputSystem;

ResourcesManager = CS.ResourcesManager;
GameManager = CS.GameManager;
MusicManager = CS.MusicManager;
JsonManager  = CS.JsonManager
MonoManager = CS.MonoManager
TaskManager = CS.TaskManager
EventCenter = CS.EventCenter
SceneLoadManager = CS.SceneLoadManager


JsonType = CS.JsonType;
AllEventName = CS.AllEventName  -- 事件中心的Type

PlayerDataList =  CS.PlayerDataList;
PlayerItemData = CS.PlayerItemData;


PlayerHealthSystem = CS.PlayerHealthSystem

NpcDialogue = CS.NpcDialogue
TaskData = CS.TaskData