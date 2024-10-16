using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class QuickRepair : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Quick Repair", "Reparo Veloce",
                                                        //SpellCircle.Second,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

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

            public InternalTarget(QuickRepair owner) : base(1, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon || targeted is BaseArmor)
                {
                    Item item = (Item)targeted;

                    if (item.IsChildOf(from.Backpack))
                    {
                        if (m_Owner.CheckSequence())
                        {
                            // Play repair sound and visual effects
                            from.PlaySound(0x2A);
                            from.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                            // Restore durability
                            if (item is BaseWeapon weapon)
                            {
                                weapon.HitPoints = weapon.MaxHitPoints;
                            }
                            else if (item is BaseArmor armor)
                            {
                                armor.HitPoints = armor.MaxHitPoints;
                            }

                            from.SendMessage("You have successfully repaired the item.");
                        }
                    }
                    else
                    {
                        from.SendMessage("The item must be in your backpack to repair.");
                    }
                }
                else
                {
                    from.SendMessage("You can only repair weapons or armor.");
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
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
