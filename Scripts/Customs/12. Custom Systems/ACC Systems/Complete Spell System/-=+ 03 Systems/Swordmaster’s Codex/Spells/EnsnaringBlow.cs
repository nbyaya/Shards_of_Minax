using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class EnsnaringBlow : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ensnaring Blow", "In Corp Grav",
            // SpellCircle.Fourth,
            21007,
            9406
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public EnsnaringBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(target))
            {
                if (Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Apply the effects
                Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Waist);
                target.PlaySound(0x204);

                target.SendMessage("You have been entangled by an Ensnaring Blow!");
                target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                target.PlaySound(0x205);

                // Apply debuff: Reduce target movement speed temporarily
                target.SendMessage("Your movements are hindered!");
                target.AddStatMod(new StatMod(StatType.Dex, "EnsnaringBlowDex", -20, TimeSpan.FromSeconds(10)));

                if (target is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)target;
                    pm.Say("*struggles against the ensnaring vines*");
                }
                else if (target is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)target;
                    bc.Emote("*is slowed down by the ensnaring blow*");
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private EnsnaringBlow m_Owner;

            public InternalTarget(EnsnaringBlow owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
