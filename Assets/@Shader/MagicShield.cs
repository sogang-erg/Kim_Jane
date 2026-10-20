using UnityEngine;
using System.Collections;

public class MagicShield : MonoBehaviour
{
    private Material material;

    public float speed = 5f;
    public float duration = 5f;
    public float hiddenValue = 0f;
    public float shownValue = 3f;

    private Coroutine shieldCoroutine;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("Renderer가 없습니다.", this);
            enabled = false;
            return;
        }

        material = renderer.material;

        if (!material.HasProperty("_shield"))
        {
            Debug.LogError("Material에 _shield 프로퍼티가 없습니다.", this);
            enabled = false;
            return;
        }

        material.SetFloat("_shield", hiddenValue);
        IsActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.SetSoundVolume(Define.ESound.Effect, 0.8f);
            SoundManager.Instance.Play2D(Define.ESound.Effect, "shield");
            if (shieldCoroutine != null)
                StopCoroutine(shieldCoroutine);

            shieldCoroutine = StartCoroutine(PlayShield());
        }
    }

    private IEnumerator PlayShield()
    {
        // 쉴드 활성화: 생성 시작부터 데미지 무시
        IsActive = true;

        // 쉴드 생성
        yield return AnimateShield(hiddenValue, shownValue);

        // 생성 완료 후 유지
        yield return new WaitForSeconds(duration);

        // 쉴드 소멸
        yield return AnimateShield(shownValue, hiddenValue);

        material.SetFloat("_shield", hiddenValue);

        // 쉴드 비활성화: 소멸 완료 후 데미지 허용
        IsActive = false;

        shieldCoroutine = null;
    }

    private IEnumerator AnimateShield(float start, float end)
    {
        float value = start;

        while (!Mathf.Approximately(value, end))
        {
            value = Mathf.MoveTowards(
                value,
                end,
                speed * Time.deltaTime
            );

            material.SetFloat("_shield", value);

            yield return null;
        }

        material.SetFloat("_shield", end);
    }
}