using System.Collections;
using _Scripts.Feedbacks;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Agent.Tower
{
    public class Tower : Agent
    {
        public GameObject upgradePrefab;
        public TowerStateChange TowerStateChange => _towerStateChange;
        private TowerStateChange _towerStateChange;

        private void OnEnable()
        {
            AgentBT.SetVariableValue(TowerStrings.Tower, this);
            
            if (!GetVariable(TowerStrings.ChannelEvent, out BlackboardVariable<TowerStateChange> stateChannel))
            {
                Debug.LogError($"StateChannel이 존재하지 않습니다. Target : {gameObject.name}");
                return;
            }

            _towerStateChange = stateChannel.Value;
        }
    
        public override void OnDeath()
        {
            base.OnDeath();
        }

        private GameObject _returnObj;
        public GameObject TowerUpgrade()
        {
            Feedbacks.Feedbacks feedbacks = GetModule<Feedbacks.Feedbacks>();
            if (feedbacks != null)
            {
                FeedbackPlayer player = feedbacks.GetFeedbackPlayer(FeedbackType.UPGRADE);
                player.FeedbackPlay();
            }
            TowerStateChange.SendEventMessage(TowerState.SHUTDOWN);
            
            _returnObj = Instantiate(upgradePrefab, transform.position, Quaternion.identity);
            _returnObj.SetActive(false);
            StartCoroutine(SpawnTower());
            return _returnObj;
        }

        public void ShutDownThreeSecond()
        {
            StartCoroutine(ShutdownTower());
        }

        private IEnumerator ShutdownTower()
        {
            TowerStateChange.SendEventMessage(TowerState.SHUTDOWN);
            yield return new WaitForSeconds(4f);
            TowerStateChange.SendEventMessage(TowerState.IDLE);
        }

        private IEnumerator SpawnTower()
        {
            yield return new WaitForSeconds(4f);
            OnDeath();
            _returnObj.SetActive(true);
            Destroy(gameObject);
        }
        
        public void ShutDownTower()
        {
            Feedbacks.Feedbacks feedbacks = GetModule<Feedbacks.Feedbacks>();
            if (feedbacks != null)
            {
                FeedbackPlayer player = feedbacks.GetFeedbackPlayer(FeedbackType.UPGRADE);
                player.FeedbackPlay();
            }
            TowerStateChange.SendEventMessage(TowerState.SHUTDOWN);
            GetModule<ITargetCaster>().ResetTargets();
        }
    }
    public static class TowerStrings
    {
        public const string Tower = "Tower";
        public const string ChannelEvent = "TowerStateChange";
    }
}

