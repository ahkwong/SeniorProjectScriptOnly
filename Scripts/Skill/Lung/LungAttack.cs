namespace Seniors.Skills.Lung
{
    public class LungAttack : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            anim.SetTrigger("Attack");
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Attack_Shoot":
                    ShootProjectile();
                    break;
            }
        }
    }
}