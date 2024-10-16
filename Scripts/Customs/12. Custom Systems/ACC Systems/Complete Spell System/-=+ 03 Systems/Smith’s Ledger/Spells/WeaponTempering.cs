using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class WeaponTempering : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Weapon Tempering", "Ferox Ignis",
            21009, // Effect ID for visuals
            9305   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public WeaponTempering(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WeaponTempering m_Owner;

            public InternalTarget(WeaponTempering owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon weapon && from == m_Owner.Caster)
                {
                    if (!m_Owner.CheckSequence())
                        return;

                    // Apply weapon enhancement effects
                    m_Owner.EnhanceWeapon(weapon);
                }
                else
                {
                    from.SendMessage("You can only enhance weapons with this spell.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void EnhanceWeapon(BaseWeapon weapon)
        {
            if (weapon == null || weapon.Deleted || Caster == null)
                return;

            // Apply visual and sound effects
            Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 1153, 0);
            Caster.PlaySound(9305);

            // Add a temporary buff to increase damage or add elemental damage
            if (Utility.RandomBool())
            {
                weapon.WeaponAttributes.HitFireArea += 20; // Adds fire elemental damage
                Caster.SendMessage("Your weapon is imbued with fiery power!");
            }
            else
            {
                weapon.Attributes.WeaponDamage += 20; // Increase overall damage
                Caster.SendMessage("Your weapon's damage has been enhanced!");
            }

            // Timer to revert the weapon enhancement
            Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveEnhancement(weapon));
        }

        private void RemoveEnhancement(BaseWeapon weapon)
        {
            if (weapon == null || weapon.Deleted)
                return;

            weapon.WeaponAttributes.HitFireArea = Math.Max(0, weapon.WeaponAttributes.HitFireArea - 20);
            weapon.Attributes.WeaponDamage = Math.Max(0, weapon.Attributes.WeaponDamage - 20);
            Caster.SendMessage("The effects of the Weapon Tempering spell have worn off.");
        }
    }
}
