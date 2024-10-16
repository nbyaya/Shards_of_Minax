using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class GrimHarvest : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Grim Harvest", "Reco Collectus",
            //SpellCircle.Fifth,
            21004,
            9300,
            false,
            Reagent.BatWing,
            Reagent.GraveDust
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public GrimHarvest(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private GrimHarvest m_Owner;

            public InternalTarget(GrimHarvest owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile && targeted != from)
                {
                    Mobile target = (Mobile)targeted;

                    if (target.Alive)
                    {
                        from.SendMessage("Your target is still alive!");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        m_Owner.HarvestEssence(from, target);
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendMessage("You must target a defeated enemy to harvest its essence.");
                }
            }
        }

        public void HarvestEssence(Mobile caster, Mobile target)
        {
            Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 0, 0);
            Effects.PlaySound(target.Location, target.Map, 0x1FB);

            int range = 3; // Effect range
            List<Mobile> toAffect = new List<Mobile>();

            foreach (Mobile m in target.GetMobilesInRange(range))
            {
                if (m == caster)
                    continue;

                if (m.Alive)
                    toAffect.Add(m);
            }

            foreach (Mobile m in toAffect)
            {
                m.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist);
                m.PlaySound(0x1F7);
                caster.Hits += Utility.RandomMinMax(10, 20);
                caster.Mana += Utility.RandomMinMax(5, 15);
                m.SendMessage("You feel your life force being drained!");
            }

            caster.SendMessage("You absorb the essence of your defeated enemies!");
            Effects.SendLocationEffect(caster.Location, caster.Map, 0x376A, 10, 10, 1109, 0);
            caster.PlaySound(0x208);

            // Temporary Buff
            BuffInfo.AddBuff(caster, new BuffInfo(BuffIcon.ReactiveArmor, 1075812, 1075813, TimeSpan.FromSeconds(30), caster, "Grim Harvest: Increased Damage and Speed"));
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
