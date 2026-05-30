using System;
using _Script.ScriptableObject.Event;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Scripts.UI
{
    public class DecalViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ShowEventChannelSO { get; private set; }

        private DecalProjector[] _decals;

        private void Awake()
        {
            _decals = GetComponentsInChildren<DecalProjector>(true);
            foreach (DecalProjector decal in _decals)
            {
                decal.enabled = false;
            }
            
            ShowEventChannelSO.AddListener<DecalShow>(HandleDecalShow);
        }

        [ContextMenu("Act True")]
        public void RaiseEvt()
        {
            ShowEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
        }
        [ContextMenu("Act False")]
        public void RaiseEvtTwo()
        {
            ShowEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
        }

        private void OnDestroy()
        {
            ShowEventChannelSO.RemoveListener<DecalShow>(HandleDecalShow);
        }

        private void HandleDecalShow(DecalShow evt)
        {
            foreach (DecalProjector decal in _decals)
            {
                decal.enabled = evt.Show;
            }
        }
    }
}
