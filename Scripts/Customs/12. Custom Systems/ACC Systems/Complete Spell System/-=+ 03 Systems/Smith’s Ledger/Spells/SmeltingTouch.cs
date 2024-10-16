using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class SmeltingTouch : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Smelting Touch", "Reparo Ferrum",
            21011, // Icon
            9307   // Cast Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public SmeltingTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SmeltingTouch m_Owner;

            public InternalTarget(SmeltingTouch owner) : base(1, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseArmor armor)
                {
                    if (!armor.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The item must be in your backpack to repair it.");
                        return;
                    }

                    if (armor.HitPoints >= armor.MaxHitPoints)
                    {
                        from.SendMessage("That item is already at full durability.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        int resourceCost = 2; // Adjust this as needed
                        if (from.Backpack.ConsumeTotal(typeof(IronIngot), resourceCost))
                        {
                            armor.HitPoints = armor.MaxHitPoints;

                            from.PlaySound(0x2A);
                            Effects.SendLocationParticles(
                                EffectItem.Create(armor.Location, armor.Map, EffectItem.DefaultDuration),
                                0x3709, 10, 30, 5052
                            );

                            from.SendMessage("You have successfully repaired the item using Smelting Touch!");
                        }
                        else
                        {
                            from.SendMessage("You do not have enough resources to repair the item.");
                        }
                    }
                }
                else if (targeted is BaseWeapon weapon)
                {
                    if (!weapon.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The item must be in your backpack to repair it.");
                        return;
                    }

                    if (weapon.HitPoints >= weapon.MaxHitPoints)
                    {
                        from.SendMessage("That item is already at full durability.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        int resourceCost = 2; // Adjust this as needed
                        if (from.Backpack.ConsumeTotal(typeof(IronIngot), resourceCost))
                        {
                            weapon.HitPoints = weapon.MaxHitPoints;

                            from.PlaySound(0x2A);
                            Effects.SendLocationParticles(
                                EffectItem.Create(weapon.Location, weapon.Map, EffectItem.DefaultDuration),
                                0x3709, 10, 30, 5052
                            );

                            from.SendMessage("You have successfully repaired the item using Smelting Touch!");
                        }
                        else
                        {
                            from.SendMessage("You do not have enough resources to repair the item.");
                        }
                    }
                }
                else
                {
                    from.SendMessage("You can only target a piece of armor or a weapon.");
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
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
