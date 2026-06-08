using UnityEngine;

[CreateAssetMenu(fileName = "ArmorPlateCalculator", menuName = "Scriptable Objects/ArmorPlateCalculator")]
public class ArmorPlateCalculator : DamageCalculator
{
    public override float CalculateDamage(float attackPower, float defensePower)
    {
        return attackPower * (100f / (100f + defensePower));
        
        /*
         * 이 방식은 방어력이 높아질수록 공격력이 비율에 따라 감쇠하는 구조입니다. 방어력
           이 0이면 공격력이 그대로 적용되지만, 방어 수치가 올라갈수록 최종 데미지가 점점
           줄어들게 됩니다. 단, 방어력이 아무리 높아져도 데미지가 완전히 0이 되지는 않습니
           다.
         */
    }
}
