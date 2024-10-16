using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ShieldingAura : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shielding Aura", "Shieldum Aurus",
            //SpellCircle.Eighth,
            21005,
            9301
        );

        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 30; } }

        private static readonly int[] ResistBonuses = { 10, 10, 10, 10, 10 }; // Bonus to all resists
        private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(30.0);

        public ShieldingAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                ApplyShieldingAura(Caster);

                List<Mobile> followers = GetFollowersInRange(Caster, 4);
                foreach (Mobile follower in followers)
                {
                    ApplyShieldingAura(follower);
                }

                // Visual and sound effects
                Caster.PlaySound(0x5C4); // Sound effect
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008); // Sparkle effect

                FinishSequence();
            }
        }

        private void ApplyShieldingAura(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            // Apply resist bonuses
            for (int i = 0; i < ResistBonuses.Length; i++)
            {
                target.Resistances[i] += ResistBonuses[i];
            }

            target.SendMessage(0x44, "You feel a protective aura surrounding you.");

            // Timer to remove the effect after duration
            Timer.DelayCall(EffectDuration, () =>
            {
                if (target == null || target.Deleted || !target.Alive)
                    return;

                for (int i = 0; i < ResistBonuses.Length; i++)
                {
                    target.Resistances[i] -= ResistBonuses[i];
                }

                target.SendMessage(0x44, "The protective aura fades away.");
            });
        }

        private List<Mobile> GetFollowersInRange(Mobile caster, int range)
        {
            List<Mobile> followers = new List<Mobile>();

            foreach (Mobile m in caster.GetMobilesInRange(range))
            {
                if (m is BaseCreature creature && creature.Controlled && creature.ControlMaster == caster)
                {
                    followers.Add(m);
                }
            }

            return followers;
        }
    }
}
