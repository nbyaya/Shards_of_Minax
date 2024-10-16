using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class HealingSalve : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Salve", "Salvus Wound",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }
        
		public override double CastDelay { get { return 0.5; } } // Short cast delay to fit the "fast strikes" description
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public HealingSalve(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanBeBeneficial(target))
            {
                if (CheckBSequence(target))
                {
                    Caster.DoBeneficial(target);

                    // Visual and Sound effects for Healing Salve
                    target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    target.PlaySound(0x1F2);

                    // Heal and remove minor debuffs
                    int toHeal = Utility.RandomMinMax(15, 30); // Heal a random amount between 15 and 30 HP
                    target.Heal(toHeal, Caster);

                    RemoveMinorDebuffs(target);

                    Caster.SendMessage("You apply the Healing Salve.");
                    target.SendMessage("You feel a soothing sensation as your wounds heal and debuffs are lifted.");
                }
            }

            FinishSequence();
        }

        private void RemoveMinorDebuffs(Mobile target)
        {
            // Example of removing minor debuffs like Poison and Weakness
            target.CurePoison(Caster); // Removes poison
            target.Paralyzed = false;  // Removes paralysis

            // Add more debuff removals as needed
            // Remove any negative StatMod (weakness effects)
            StatMod[] mods = target.StatMods.ToArray();
            foreach (StatMod mod in mods)
            {
                if (mod.Name == "Weakness" || mod.Name == "Clumsy" || mod.Name == "Feeblemind")
                {
                    target.RemoveStatMod(mod.Name);
                }
            }
        }

        private class InternalTarget : Target
        {
            private HealingSalve m_Owner;

            public InternalTarget(HealingSalve owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
