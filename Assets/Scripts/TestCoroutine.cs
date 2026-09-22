using UnityEngine;
using System.Collections;

public class TestCoroutine : MonoBehaviour
{
    void Start()
    {
        TestFunc(); // 호출
        // StartCoroutine(CoTestFunc()); // 호출
        
    }
     Coroutine coTest; // 코루틴 참조 저장용

    public void TestFunc()
    {
        if (coTest != null) // 이미 코루틴이 실행 중이면 중지: 중복 방지
            StopCoroutine(coTest);

        coTest = StartCoroutine(CoTestFunc()); // 코루틴 호출
    }

    // void TestFunc() // 단발적 실행
    // {
    //     for (int i = 0; i < 1000; i++)
    //     {
    //         Debug.Log("TestFunc" + i);
    //     }
    // }

    IEnumerator CoTestFunc() // 코루틴으로 실행
    {
        for (int i = 0; i < 10000000; i++)
        {
            Debug.Log("TestFunc" + i);

            if (i % 100 == 0)
                yield return null; // 한 프레임 대기
                // yield return new WaitForSeconds(1f); // 1초 대기
        }
        yield return null;
    }

}
