using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class MathfTool
{
    #region �ǶȻ���
    /// <summary>
    /// �Ƕ�ת����
    /// </summary>
    /// <param name="deg">�Ƕ�</param>
    /// <returns>����ֵ</returns>
    public static float DegToRad(float deg)
    {
        return Mathf.Deg2Rad * deg;
    }
    /// <summary>
    /// ����ת�Ƕ�
    /// </summary>
    /// <param name="deg">����</param>
    /// <returns>�Ƕ�ֵ</returns>
    public static float RadToDeg(float rad)
    {
        return Mathf.Rad2Deg * rad;
    }
    #endregion

    #region ������
    /// <summary>
    /// ���� ��XZƽ��ľ��� --3D
    /// </summary>
    /// <param name="StartPoint">��ʼ��</param>
    /// <param name="EndPoint">�յ�</param>
    /// <returns>����</returns>
    public static float DistanceXZ(Vector3 StartPoint,Vector3 EndPoint) {
        StartPoint.y = 0;
        EndPoint.y = 0;
        return Vector3.Distance(StartPoint, EndPoint);
    }
    /// <summary>
    /// ���� ��XZƽ��ľ��� --2D
    /// </summary>
    /// <param name="StartPoint">��ʼ��</param>
    /// <param name="EndPoint">�յ�</param>
    /// <returns>����</returns>
    public static float DistanceXY(Vector3 StartPoint, Vector3 EndPoint)
    {
        StartPoint.z = 0;
        StartPoint.z = 0;
        return Vector3.Distance(StartPoint, EndPoint);
    }
    #endregion

    #region ������
    /// <summary>
    /// �����Ƿ�����Ļ�� true ���� ��false û����
    /// </summary>
    /// <param name="point">����</param>
    /// <returns></returns>
    public static bool IsOutSceen(Vector3 point) {
        Vector3 srceenPoint = Camera.main.WorldToScreenPoint(point);
        if (srceenPoint.x >= 0 && srceenPoint.x <= Screen.width && srceenPoint.y >= 0 && srceenPoint.y <= Screen.height)
            return false;
        return true;
    }
    /// <summary>
    /// �ж��Ƿ������η�Χ��
    /// </summary>
    /// <param name="centerPoint">���ĵ�Բ��</param>
    /// <param name="forward">������</param>
    /// <param name="targetPoint">Ŀ��λ��</param>
    /// <param name="sectorRadius">���ΰ뾶</param>
    /// <param name="angle">���Ƕ�</param>
    /// <returns></returns>
    public static bool IsInSectorRangeXZ(Vector3 centerPoint,Vector3 forward,Vector3 targetPoint,float sectorRadius,float angle) { 
        centerPoint.y = 0;
        forward.y = 0;
        targetPoint.y = 0;
        return DistanceXZ(centerPoint, targetPoint) <= sectorRadius && Vector3.Angle(forward, targetPoint-centerPoint) <= angle / 2;
    }

    /// <summary>
    /// ���߼�� GameObject�����ϵĽű���RayCastHIt
    /// </summary>
    /// <param name="ray">����</param>
    /// <param name="T">��Ҫ������ GameObject�����ϵĽű���RayCastHIt</param>
    /// <param name="callback">�ص�����</param>
    /// <param name="distance">����</param>
    /// <param name="LayerMask">�㼶</param>
    public static void RayCast<T>(Ray ray, UnityAction<T> callback, float distance, int LayerMask) where T: class
    {
        Type type = typeof(T);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, LayerMask))
        {
            if (type == typeof(GameObject))
                callback.Invoke(hitInfo.collider.gameObject as T);
            else
                callback.Invoke(hitInfo.collider.GetComponent<T>());
        }
    }

    public static void RayCast(Ray ray, UnityAction<RaycastHit> callback, float distance, int LayerMask)
    {
      
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, LayerMask))
        {           
            callback.Invoke(hitInfo);         
        }
    }

    /// <summary>
    /// ���߼������  GameObject ���� ���ϵĽű� ���� RayCastHIt
    /// </summary>
    /// <param name="ray">����</param>
    /// <param name="T">��Ҫ������ GameObject�����ϵĽű���RayCastHIt</param>
    /// <param name="callback">�ص�����</param>
    /// <param name="distance">����</param>
    /// <param name="LayerMask">�㼶</param>
    public static void RayCastAll<T>(Ray ray, UnityAction<T> callback, float distance, int LayerMask) where T : class
    {
        Type type =  typeof(T);
        RaycastHit[] hitInfo = Physics.RaycastAll(ray, distance, LayerMask);
        foreach (RaycastHit hit in hitInfo)
        {
            if (type == typeof(GameObject))
                callback.Invoke(hit.collider.gameObject as T);
            else
                callback.Invoke(hit.collider.GetComponent<T>());           
        }
    }
    /// <summary>
    /// 射线检测得到所有的Raycasthit
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="callback">回调</param>
    /// <param name="distance">距离</param>
    /// <param name="LayerMask">层级</param>    
    public static void RayCastAll(Ray ray, UnityAction<RaycastHit> callback, float distance, int LayerMask) 
    {
        
        RaycastHit[] hitInfo = Physics.RaycastAll(ray, distance, LayerMask);
        foreach (RaycastHit hit in hitInfo)
        {
           
            callback.Invoke(hit);
                  
        }
    }


    //QueryTriggerInteraction.Collide ����������, Ignore ������������, UseGlobal ʹ��Setting���������

    /// <summary>
    /// ���η�Χ��� 
    /// </summary>
    /// <typeparam name="T">��Ҫ������ GameObject��Collider���Լ����ϵĽű�����</typeparam>
    /// <param name="centerPoint">���ĵ�</param>
    /// <param name="boxInfo">���εĳ�����</param>
    /// <param name="rotation">��ת��Ϣ</param>
    /// <param name="layerMask">�㼶</param>
    /// <param name="callBack">�ص�</param>
    public static void OverlapBox<T>(Vector3 centerPoint,Vector3 boxInfo,Quaternion rotation,int layerMask,UnityAction<T> callBack) where T: class 
    {
        Type type = typeof(T);  
        Collider[] colliders = Physics.OverlapBox(centerPoint, boxInfo, rotation, layerMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (type == typeof(GameObject))
                callBack.Invoke(collider.gameObject as T);
            else if (type == typeof(Collider))
                callBack(collider as T);
            else
                callBack(collider.gameObject.GetComponent<T>());

        }
    }

    /// <summary>
    /// ���η�Χ��� 
    /// </summary>
    /// <typeparam name="T">��Ҫ������ GameObject��Collider���Լ����ϵĽű�����</typeparam>
    /// <param name="centerPoint">���ĵ�</param>
    /// <param name="radius">�뾶</param>
    /// <param name="layerMask">�㼶</param>
    /// <param name="callBack">�ص�</param>
    public static void OverlapSphere<T>(Vector3 centerPoint, float radius, int layerMask, UnityAction<T> callBack) where T : class
    {
        Type type = typeof(T);
        Collider[] colliders = Physics.OverlapSphere(centerPoint, radius, layerMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (type == typeof(GameObject))
                callBack.Invoke(collider.gameObject as T);
            else if (type == typeof(Collider))
                callBack(collider as T);
            else
                callBack(collider.gameObject.GetComponent<T>());

        }
    }
    #endregion
}
