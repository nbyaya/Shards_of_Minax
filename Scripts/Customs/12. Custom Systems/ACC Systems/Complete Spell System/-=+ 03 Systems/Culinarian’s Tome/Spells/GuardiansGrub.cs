using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class GuardiansGrub : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Guardian's Grub", "Guard me grub!",
            21004, // Icon ID
            9300   // Cast sound
        );

        public override SpellCircle Circle => SpellCircle.First;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 20;

        public GuardiansGrub(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Item grub = new GuardiansGrubItem();
                Caster.AddToBackpack(grub);
                Caster.SendMessage("You have summoned a Guardian's Grub!");
                Caster.PlaySound(0x212); // Play a summoning sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x375A, 1, 30, 1153, 7, 9917, 0); // Play visual effect
            }

            FinishSequence();
        }
    }

    public class GuardiansGrubItem : Item
    {
        public GuardiansGrubItem() : base(0x160B) // Item ID for the grub
        {
            Name = "Guardian's Grub";
            Hue = 1161; // Color the grub
            Weight = 1.0;
        }

        public GuardiansGrubItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // Must be in your backpack to use it.
                return;
            }

            from.SendMessage("You consume the Guardian's Grub and feel a surge of protection!");
            from.PlaySound(0x1F2); // Play a consumption sound
            Effects.SendTargetParticles(from, 0x373A, 1, 15, 1153, 7, 9502, 0, 0); // Visual effect on the player

            from.VirtualArmorMod += 10; // Increase player's armor temporarily

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                from.VirtualArmorMod -= 10;
                from.SendMessage("The effect of the Guardian's Grub wears off.");
            });

            Delete(); // Remove the grub item after use
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
