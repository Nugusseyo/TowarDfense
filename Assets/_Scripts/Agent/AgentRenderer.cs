using System.Collections;
using _Script.Agent.Modules;
using UnityEngine;
// 💡 코루틴(IEnumerator) 사용을 위해 추가

namespace _Scripts.Agent
{
    public class AgentRenderer : MonoBehaviour, IModule, IAgentRenderer // 💡 필요시 상속 구조(IModule 등)를 다시 붙여주세요!
    {
        public GameObject GameObject => gameObject; 
        private Agent _moduleAgent;
        private Animator _animator;
        public Animator Animator => _animator;

        // 💡 피격 깜빡임 연출을 위한 내부 변수들
        private Renderer[] _renderers;
        private MaterialPropertyBlock _mpb;
        private Coroutine _flashCoroutine;

        // 💡 Unitychan-Toonshader(UTS2)의 에미션 컬러 셰이더 프로퍼티 이름
        private static readonly int EmissiveColorProp = Shader.PropertyToID("_Emissive_Color");

        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent as Agent;
            _animator = GetComponentInChildren<Animator>();

            // 💡 최초 생성 시 본체 및 자식 오브젝트의 모든 렌더러 미리 캐싱
            _renderers = GetComponentsInChildren<Renderer>();
            _mpb = new MaterialPropertyBlock();
        }

        public void PlayFadeAcrossClip(int clipSource, float duration)
        {
            _animator.CrossFadeInFixedTime(clipSource, duration);
        }

        public void PlayClip(int clipSource)
        {
            _animator.PlayInFixedTime(clipSource, 0, 0f);
            _animator.Update(0f); // 이거 하면 애니메이션 프레임 안기다리고 강제로 Update 시켜버린대용
        }

        public void SetAnimatorFloat(int id, float value) => _animator.SetFloat(id, value);

        #region Hit Flash (UTS2 Emission)

        /// <summary>
        /// 외부에서 캐릭터가 맞았을 때 호출하는 피격 깜빡임 메서드
        /// </summary>
        public void PlayHitFlash(Color flashColor, float flashTime = 0.08f, int count = 2)
        {
            // 연타로 맞았을 때 이전 깜빡임 연출이 도는 중이면 끄고 새로 시작
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            _flashCoroutine = StartCoroutine(HitFlashCoroutine(flashColor, flashTime, count));
        }

        private IEnumerator HitFlashCoroutine(Color flashColor, float flashTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                // 에미션 켜기
                SetEmissiveColor(flashColor);
                yield return new WaitForSeconds(flashTime);

                // 에미션 끄기 (UTS2는 검은색을 주면 불이 꺼집니다)
                SetEmissiveColor(Color.black);
                yield return new WaitForSeconds(flashTime);
            }

            _flashCoroutine = null;
        }

        private void SetEmissiveColor(Color color)
        {
            if (_renderers == null) return;

            foreach (var r in _renderers)
            {
                // 리서칭이나 오브젝트 풀 과정에서 혹시 모를 Null 방어
                if (r == null) continue; 

                r.GetPropertyBlock(_mpb);
                _mpb.SetColor(EmissiveColorProp, color);
                r.SetPropertyBlock(_mpb);
            }
        }

        #endregion
    }
}