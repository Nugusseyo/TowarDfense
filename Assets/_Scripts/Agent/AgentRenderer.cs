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
        private Renderer[] _renderers;
        private MaterialPropertyBlock _mpb;
        private Coroutine _flashCoroutine;
        
        private static readonly int EmissiveColorProp = Shader.PropertyToID("_Emissive_Color");
        private static readonly int SubEmissiveColor = Shader.PropertyToID("_EmissionColor");
        [SerializeField] private bool isUsingExtraShader;
        private int _realEmissiveColorProp;

        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent as Agent;
            _animator = GetComponentInChildren<Animator>();

            _renderers = GetComponentsInChildren<Renderer>();
            _mpb = new MaterialPropertyBlock();
            
            _realEmissiveColorProp = isUsingExtraShader ? EmissiveColorProp : SubEmissiveColor;
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
        public void PlayHitFlash(Color flashColor, float flashTime = 0.08f, int count = 2)
        {
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
                SetEmissiveColor(flashColor);
                yield return new WaitForSeconds(flashTime);
                
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
                if (r == null) continue; 

                r.GetPropertyBlock(_mpb);
                _mpb.SetColor(EmissiveColorProp, color);
                r.SetPropertyBlock(_mpb);
            }
        }

        #endregion
    }
}