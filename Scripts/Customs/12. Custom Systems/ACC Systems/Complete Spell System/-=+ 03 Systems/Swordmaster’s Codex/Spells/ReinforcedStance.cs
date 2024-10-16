using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class ReinforcedStance : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Reinforced Stance", "Fortis Stance",
            //SpellCircle.Second,
            21013,
            9412
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ReinforcedStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Applying the defensive stance effect
                Caster.SendMessage("You assume a reinforced stance, ready to withstand heavy attacks!");
                Caster.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist); // Visual effect: Blue energy around the caster
                Caster.PlaySound(0x1ED); // Sound effect: A strong shield-like sound

                // Increasing the caster's physical and magical resistances
                double duration = 10.0 + (Caster.Skills[CastSkill].Value / 5.0); // Duration scales with skill level
                int physicalBonus = 20; // Flat bonus to physical resistance
                int magicalBonus = 15; // Flat bonus to magical resistance

                // Create and apply resistance mods
                ResistanceMod physMod = new ResistanceMod(ResistanceType.Physical, physicalBonus);

                
                Caster.AddResistanceMod(physMod);


                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    // Remove the resistance mods after the duration ends
                    Caster.RemoveResistanceMod(physMod);

                    Caster.SendMessage("Your reinforced stance fades away.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
