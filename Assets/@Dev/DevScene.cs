using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Random = UnityEngine.Random;
using System.Collections;

public class DevScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.DevScene;
        // TODO: 개발 씬 초기화 로직 작성
        ResourceManager.Instance.LoadAll();
        DataManager.Instance.LoadData();

        SaveManager.Instance.Load();
        SaveManager.Instance.StartAutoSave();

        // // SoundManager.Instance.Play2D(Define.ESound.Bgm, "bgm");
        // StartCoroutine(CoPlaySound());

    }

    // IEnumerator CoPlaySound()
    // {

    //     while (true)
    //     {
    //         yield return new WaitForSeconds(5);
    //         SoundManager.Instance.Play3D("explosion", gameObject, 1, 50);
    //     }
    // }
}
