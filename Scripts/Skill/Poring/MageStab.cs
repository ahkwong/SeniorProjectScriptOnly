namespace Seniors.Skills.Poring
{
    public class MageStab : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;


            anim.SetTrigger("Stab");
        }     

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Stab_Shoot":
                    ShootProjectile();
                    break;
            }
        }
    }
}