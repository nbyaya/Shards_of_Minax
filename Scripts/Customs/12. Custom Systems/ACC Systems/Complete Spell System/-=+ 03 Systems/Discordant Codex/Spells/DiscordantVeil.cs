using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordantVeil : DiscordanceSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Discordant Veil", "Enshroud",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Third;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 30;

        public DiscordantVeil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Visual effect
                Caster.PlaySound(0x1FB); // Sound effect
                
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3)) // Get all mobiles within 3 tiles
                {
                    if (m is BaseCreature bc && bc.ControlMaster == Caster || m == Caster) // Check if the mobile is the caster or a controlled follower
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    ApplyResistanceBonus(m);
                    m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist); // Flashy effect for each target
                    m.SendMessage("You feel a surge of protective energy."); // Notify the player
                }

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RemoveResistanceBonus(targets)); // Effect lasts for 30 seconds
            }

            FinishSequence();
        }

        private void ApplyResistanceBonus(Mobile m)
        {
            m.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, 10));
            m.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, 10));
            m.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, 10));
            m.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, 10));
            m.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, 10));
        }

        private void RemoveResistanceBonus(List<Mobile> targets)
        {
            foreach (Mobile m in targets)
            {
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 10));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, 10));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, 10));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, 10));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, 10));
                m.SendMessage("The protective energy fades away.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
