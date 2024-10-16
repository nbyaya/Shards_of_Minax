using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class HarmonyOfRestoration : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Harmony of Restoration", "Revito Harmonia",
                                                        21004, // Sound ID for spell casting
                                                        9300 // Visual effect ID for casting
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 75; } }

        public HarmonyOfRestoration(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !target.Alive)
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen or is dead.
                return;
            }

            if (SpellHelper.CheckTown(target, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);
                
                // Play sound and visual effects
                Effects.PlaySound(Caster.Location, Caster.Map, 21004); // Play casting sound
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Green Swirling Particles

                // Restoration effect (randomly restores either mana or stamina)
                bool restoreMana = Utility.RandomBool();
                int amountRestored = (int)(Caster.Skills[CastSkill].Value * 0.25); // Restoration amount based on skill level

                if (restoreMana)
                {
                    target.Mana = Math.Min(target.Mana + amountRestored, target.ManaMax);
                    target.SendMessage(0x35, "A revitalizing melody restores your mana!"); // Message to the target
                }
                else
                {
                    target.Stam = Math.Min(target.Stam + amountRestored, target.StamMax);
                    target.SendMessage(0x35, "A revitalizing melody restores your stamina!"); // Message to the target
                }

                // Additional flashy effect
                // Corrected line: Use `EffectLayer` enum for EffectLayer parameter
                Effects.SendTargetParticles(target, 0x376A, 1, 17, 1109, 0, (int)EffectLayer.Head); // More particles at target's head
                target.PlaySound(0x1F2); // Sound when target is affected
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HarmonyOfRestoration m_Owner;

            public InternalTarget(HarmonyOfRestoration owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
