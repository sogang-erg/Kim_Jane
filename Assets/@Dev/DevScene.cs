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

        // AdsManager.Instance.Init();
        // StartCoroutine(CoShowAds());

        IAPManager.Instance.Init();
        StartCoroutine(CoBuyItem());
    }

    // IEnumerator CoShowAds()
    // {
    //     yield return new WaitForSeconds(5);
    //     AdsManager.Instance.ShowRewardedAds(() =>
    //     {
    //         Debug.Log("Rewarded ad completed.");
    //         // TODO: 보상 지급 로직 작성
    //     });
    // }

    IEnumerator CoBuyItem()
    {
        yield return new WaitForSeconds(5);
        IAPManager.Instance.Purchase("com.rookiss.s2.noads", () =>
        {
            Debug.Log("Purchase completed.");
            // TODO: 보상 지급 로직 작성
        });
    }
}
