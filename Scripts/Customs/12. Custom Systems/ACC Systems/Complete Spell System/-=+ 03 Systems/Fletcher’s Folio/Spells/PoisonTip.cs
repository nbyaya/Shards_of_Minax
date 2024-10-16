using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class PoisonTip : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Poison Tip", "Noxus!",
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 18; } }

        private static int poisonDuration = 30; // Duration of poison effect in seconds
        private static int poisonLevel = 2; // Poison level (0 = Lesser, 1 = Regular, 2 = Greater, etc.)
        private static int maxShots = 3; // Maximum number of poisoned shots

        private int currentShots;

        public PoisonTip(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
            currentShots = maxShots;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("Your arrows are now coated with a potent poison!");

                // Apply the poison effect to the caster
                Caster.BeginAction(typeof(PoisonTip)); // Prevent multiple poison effects
                Timer.DelayCall(TimeSpan.FromSeconds(poisonDuration), () => Caster.EndAction(typeof(PoisonTip)));
                
                // Replace all arrows with custom arrows
                ReplaceArrowsWithCustomArrows();
            }

            FinishSequence();
        }

        private void ReplaceArrowsWithCustomArrows()
        {
            foreach (Item item in Caster.Backpack.FindItemsByType(typeof(Arrow)))
            {
                if (item is Arrow)
                {
                    item.Delete();
                }
            }
        }

        public bool CanUseArrow(Mobile from)
        {
            return from == Caster && currentShots > 0;
        }

        public void ApplyPoison(Mobile target)
        {
            Poison poison = Poison.GetPoison(poisonLevel);
            target.ApplyPoison(Caster, poison);
            target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
            target.PlaySound(0x1E1);

            Caster.SendMessage("You poison your target with a deadly toxin!");
        }

        public void DecrementShots()
        {
            currentShots--;
            if (currentShots <= 0)
            {
                // Handle the case when no more shots are available
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
