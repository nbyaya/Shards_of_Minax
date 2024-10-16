using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting; // Correct namespace for Target class

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class TailorsTouch : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tailor's Touch", "Ex Fabricatus",
            21010, 9300
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public TailorsTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.SendMessage("Select an item to enhance.");
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TailorsTouch m_Owner;

            public InternalTarget(TailorsTouch owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseArmor || targeted is BaseWeapon || targeted is BaseClothing)
                {
                    Item item = (Item)targeted;

                    if (!from.InRange(item.GetWorldLocation(), 1))
                    {
                        from.SendLocalizedMessage(500446); // That is too far away.
                        return;
                    }

                    if (from.Backpack != null && from.Backpack.FindItemByType(typeof(TailoringSpellbook)) != null)
                    {
                        if (m_Owner.CheckSequence())
                        {
                            from.PlaySound(0x1F2);
                            Effects.SendTargetParticles(from, item.Serial, 0x373A, 1, 29, 99, 0, EffectLayer.Waist, 0);
                            from.SendMessage("You enhance the item with a magical touch!");

                            ApplyEnhancement(item, from);

                            m_Owner.FinishSequence();
                        }
                    }
                    else
                    {
                        from.SendMessage("You need a Tailor's Workbook to cast this spell.");
                    }
                }
                else
                {
                    from.SendMessage("This spell can only be cast on items of clothing, armor, or weapons.");
                }
            }

            private static void ApplyEnhancement(Item item, Mobile caster)
            {
                int effect = Utility.Random(3);

                if (item is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)item;

                    switch (effect)
                    {
                        case 0:
                            weapon.Attributes.WeaponDamage += 5;
                            caster.SendMessage("The weapon glows with a magical aura, enhancing its damage.");
                            break;
                        case 1:
                            weapon.Attributes.SpellChanneling = 1;
                            caster.SendMessage("The weapon hums softly, allowing spells to be cast without interruption.");
                            break;
                        case 2:
                            weapon.Attributes.CastSpeed += 1;
                            caster.SendMessage("The weapon becomes lighter, allowing faster spell casting.");
                            break;
                    }
                }
                else if (item is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)item;

                    switch (effect)
                    {
                        case 0:
                            armor.Attributes.BonusHits += 5;
                            caster.SendMessage("The armor shimmers, fortifying your resilience.");
                            break;
                        case 1:
                            armor.Attributes.LowerManaCost += 5;
                            caster.SendMessage("The armor feels lighter, reducing your mana consumption.");
                            break;
                        case 2:
                            armor.Attributes.ReflectPhysical += 5;
                            caster.SendMessage("The armor hardens, reflecting physical attacks.");
                            break;
                    }
                }
                else if (item is BaseClothing)
                {
                    BaseClothing clothing = (BaseClothing)item;

                    switch (effect)
                    {
                        case 0:
                            clothing.Attributes.RegenMana += 2;
                            caster.SendMessage("The clothing sparkles, enhancing your mana regeneration.");
                            break;
                        case 1:
                            clothing.Attributes.LowerRegCost += 10;
                            caster.SendMessage("The clothing feels more efficient, reducing reagent usage.");
                            break;
                        case 2:
                            clothing.Attributes.CastRecovery += 1;
                            caster.SendMessage("The clothing flows more freely, improving your casting recovery.");
                            break;
                    }
                }

                item.Hue = 1153; // Magical blue hue
            }
        }
    }
}
