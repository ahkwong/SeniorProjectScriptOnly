namespace Seniors.Skills.Poring
{
    public class MageSlash : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;


            anim.SetTrigger("Slash");
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Slash_Shoot":
                    ShootProjectile();
                    break;
            }
        }
    }
}