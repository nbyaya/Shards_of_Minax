using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class QuickRepair : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Repair", "Repairus Instantus",
            // SpellCircle.First,
            21008,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public QuickRepair(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private QuickRepair m_Owner;

            public InternalTarget(QuickRepair owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseClothing || o is BaseArmor)
                {
                    Item item = (Item)o;

                    // Check for specific types of items to see if they need repair
                    if (item is BaseClothing clothing && clothing.MaxHitPoints > 0 && clothing.HitPoints < clothing.MaxHitPoints)
                    {
                        if (m_Owner.CheckSequence())
                        {
                            clothing.HitPoints = clothing.MaxHitPoints;

                            from.SendMessage("You have successfully repaired the item!");
                            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5021);
                            from.PlaySound(0x1F6); // Repair sound

                            // Add a visual effect
                            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            from.FixedEffect(0x37B9, 10, 20);
                        }
                    }
                    else if (item is BaseArmor armor && armor.MaxHitPoints > 0 && armor.HitPoints < armor.MaxHitPoints)
                    {
                        if (m_Owner.CheckSequence())
                        {
                            armor.HitPoints = armor.MaxHitPoints;

                            from.SendMessage("You have successfully repaired the item!");
                            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5021);
                            from.PlaySound(0x1F6); // Repair sound

                            // Add a visual effect
                            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            from.FixedEffect(0x37B9, 10, 20);
                        }
                    }
                    else
                    {
                        from.SendMessage("That item does not need to be repaired.");
                    }
                }
                else
                {
                    from.SendMessage("You can only repair clothing or armor.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
