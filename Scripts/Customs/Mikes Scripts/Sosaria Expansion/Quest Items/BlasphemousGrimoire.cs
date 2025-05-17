using System;
using Server.Items;

namespace Server.Items
{
    public class BlasphemousGrimoire : Item
    {
        [Constructable]
        public BlasphemousGrimoire() : base(0x0FBD) // Book graphic
        {
            Hue = 2118; // Deep violet, void-scarred
            Name = "Blasphemous Grimoire";
            LootType = LootType.Blessed;
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            LabelTo(from, "*You hear whispering... but it's inside your mind.*");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage(0x22, "The moment you open the grimoire, your vision blurs…");
            from.PlaySound(0x1F3);
            from.FixedParticles(0x373A, 10, 15, 5036, 2118, 0, EffectLayer.Head);
            from.AddStatMod(new StatMod(StatType.Int, "GrimoireCurse", -10, TimeSpan.FromMinutes(2.0)));
        }

        public BlasphemousGrimoire(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
