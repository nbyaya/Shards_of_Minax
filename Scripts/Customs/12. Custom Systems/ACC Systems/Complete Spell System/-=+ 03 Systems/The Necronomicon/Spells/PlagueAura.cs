using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class PlagueAura : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Plague Aura", "Infestis Aura",
            21004,
            9300,
            false,
            Reagent.GraveDust,
            Reagent.DaemonBlood,
            Reagent.NoxCrystal
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly int Radius = 4;
        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30.0);
        private static readonly TimeSpan DamageInterval = TimeSpan.FromSeconds(2.0);

        public PlagueAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 1, 62, 9910, 1363, 0, EffectLayer.Waist);
                Caster.PlaySound(0x1FB);
                Caster.SendMessage("You are surrounded by a plague aura!");

                Timer.DelayCall(TimeSpan.Zero, DamageInterval, (int)(Duration.TotalSeconds / DamageInterval.TotalSeconds), () => ApplyAuraEffect());
            }

            FinishSequence();
        }

        private void ApplyAuraEffect()
        {
            if (Caster == null || Caster.Deleted || !Caster.Alive)
                return;

            var targets = new ArrayList();
            foreach (Mobile m in Caster.GetMobilesInRange(Radius))
            {
                if (m != Caster && m.Alive && Caster.CanBeHarmful(m, false))
                {
                    targets.Add(m);
                }
            }

            foreach (Mobile target in targets)
            {
                Caster.DoHarmful(target);
                target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                target.PlaySound(0x1FB);

                int damage = Utility.RandomMinMax(5, 10);
                target.Damage(damage, Caster);
                target.SendMessage("You are weakened by the plague aura!");

                // Apply weakening debuff
                target.AddStatMod(new StatMod(StatType.Str, "PlagueAuraStr", -5, Duration));
                target.AddStatMod(new StatMod(StatType.Dex, "PlagueAuraDex", -5, Duration));
                target.AddStatMod(new StatMod(StatType.Int, "PlagueAuraInt", -5, Duration));
            }
        }
    }
}
